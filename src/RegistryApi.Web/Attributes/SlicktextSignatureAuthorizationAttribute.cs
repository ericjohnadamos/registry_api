namespace RegistryApi.Web.Attributes;

using RegistryApi.Core;
using RegistryApi.SharedKernel.Exceptions;
using RegistryApi.Web.Features.ApiIntegrations;
using RegistryApi.Web.Features.Users;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class SlicktextSignatureAuthorizationAttribute : ActionFilterAttribute
{
    public const string INTEGRATION_CUSTOMER_ID = @"Integration-CustomerId";

    /// <inheritdoc/>
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var md5WebhookSecret = context.HttpContext.Request.Headers[@"X-Slicktext-Signature"].ToString();
        if (!md5WebhookSecret.Any())
        {
            context.Result = new UnauthorizedObjectResult(
                "You need to include the webhook secret in proper hashing");
            return;
        }

        var mediator = context.HttpContext.RequestServices.GetService(typeof(IMediator)) as IMediator;
        var apiIntegration = mediator?.Send(new GetApiIntegrationByWebhookQuery(md5WebhookSecret)).Result;
        if (apiIntegration == null)
        {
            context.Result = new BadRequestObjectResult("Invalid webhook secret");
            return;
        }

        context.HttpContext.Request.Headers.Add(INTEGRATION_CUSTOMER_ID, apiIntegration.CustomerId.ToString());

        var userOption = mediator?.Send(
            new GetUserQuery(apiIntegration.Username, apiIntegration.Password)).Result;
        userOption?.Match(
            user =>
            {
                var claims = new List<Claim>()
                {
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(ApiUserClaimTypes.Username, user.Username),
                    new(ApiUserClaimTypes.Password, user.Password),
                    new(ApiUserClaimTypes.CustomerKey, user.Customer.Key),
                    new(ApiUserClaimTypes.TimeZoneId, user.Customer?.TimeZoneId ?? ""),
                };

                foreach (var userRole in user.UsersInRole.Select(u => u.Role))
                    claims.Add(new Claim(ClaimTypes.Role, userRole.Name));

                var identity = new ClaimsIdentity(claims);
                var principal = new ClaimsPrincipal(identity);
                context.HttpContext.User = principal;
            },
            ex => AuthenticateResult.Fail(
                ex is NotFoundException ? "Invalid Username or Password" : "Invalid Authorization Header"));
    }
}
