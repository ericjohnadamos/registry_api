namespace RegistryApi.IntegrationTests.Authorisation;

using RegistryApi.Web.Roles;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Security.Claims;
using Xunit;

public class AuthorisationAttributeTests
{
    [Fact]
    public async Task AuthorisationAttributeTests_ValidClaimShouldNotFail()
    {
        // Arrange
        var authorizeFilter = new AuthorizeFilter(
            new AuthorizationPolicyBuilder().RequireClaim(ClaimTypes.Role, UserTypePolicies.Admin).Build());
        var authorizationContext = GetAuthorizationContext();

        // Act
        await authorizeFilter.OnAuthorizationAsync(authorizationContext);

        // Assert
        authorizationContext.Result.Should().BeNull();
    }

    [Fact]
    public async Task AuthorisationAttributeTests_EmptyClaimsShouldChallengeAnonymousUser()
    {
        // Arrange
        var authorizeFilter = new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());
        var authorizationContext = GetAuthorizationContext(anonymous: true);
        authorizationContext.Filters.Add(authorizeFilter);

        // Act
        await authorizeFilter.OnAuthorizationAsync(authorizationContext);

        // Assert
        authorizationContext.Result.Should().BeOfType<ChallengeResult>();
    }

    [Fact]
    public async Task AuthorisationAttributeTests_EmptyClaimsWithAllowAnonymousAttributeShouldNotRejectAnonymousUser()
    {
        // Arrange
        var authorizeFilter = new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());
        var authorizationContext = GetAuthorizationContext(anonymous: true);
        authorizationContext.Filters.Add(new AllowAnonymousFilter());

        // Act
        await authorizeFilter.OnAuthorizationAsync(authorizationContext);

        // Assert
        authorizationContext.Result.Should().BeNull();
    }

    [Fact]
    public async Task AuthorisationAttributeTests_SingleValidClaimShouldSucceed()
    {
        // Arrange
        var authorizeFilter = new AuthorizeFilter(
            new AuthorizationPolicyBuilder()
                .RequireClaim(ClaimTypes.Role, UserTypePolicies.Admin, UserTypePolicies.Security)
                .Build());
        var authorizationContext = GetAuthorizationContext();

        // Act
        await authorizeFilter.OnAuthorizationAsync(authorizationContext);

        // Assert
        authorizationContext.Result.Should().BeNull();
    }

    private AuthorizationFilterContext GetAuthorizationContext(
        bool anonymous = false,
        Action<IServiceCollection> registerServices = null)
    {
        var basicPrincipal = new ClaimsPrincipal(
            new ClaimsIdentity(
                new Claim[]
                {
                    new Claim(ClaimTypes.Role, UserTypePolicies.Admin),
                    new Claim(ClaimTypes.NameIdentifier, "John")
                },
                "Basic"));

        // ServiceProvider
        var serviceCollection = new ServiceCollection();

        var auth = new Mock<IAuthenticationService>();

        serviceCollection.AddOptions();
        serviceCollection.AddLogging();
        serviceCollection.AddSingleton(auth.Object);
        serviceCollection.AddAuthorization();
        registerServices?.Invoke(serviceCollection);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        // HttpContext
        var httpContext = new Mock<HttpContext>();
        auth.Setup(c => c.AuthenticateAsync(httpContext.Object, "Basic")).ReturnsAsync(AuthenticateResult.Success(new AuthenticationTicket(basicPrincipal, "Basic")));
        httpContext.SetupProperty(c => c.User);

        if (!anonymous)
            httpContext.Object.User = basicPrincipal;

        httpContext.SetupGet(c => c.RequestServices).Returns(serviceProvider);
        var contextItems = new Dictionary<object, object>();
        httpContext.SetupGet(c => c.Items).Returns(contextItems);
        httpContext.SetupGet(c => c.Features).Returns(Mock.Of<IFeatureCollection>());

        // AuthorizationFilterContext
        var actionContext = new ActionContext(
            httpContext: httpContext.Object,
            routeData: new RouteData(),
            actionDescriptor: new ActionDescriptor());

        var authorizationContext = new AuthorizationFilterContext(
            actionContext,
            Enumerable.Empty<IFilterMetadata>().ToList()
        );

        return authorizationContext;
    }
}
