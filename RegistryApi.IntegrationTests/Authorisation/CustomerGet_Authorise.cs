namespace RegistryApi.IntegrationTests.Authorisation;

using RegistryApi.Web;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net;
using Xunit;

public class CustomerGet_Authorise
{
    [Fact]
    public async Task GetCustomerStays_AccessingFirstRouteWithoutAuthorisation_ShouldThrowUnauthorisedException()
    {
        // Arrange
        var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
        var client = server.CreateClient();
        var url = "/api/customer/supportcommunity";

        var expected = HttpStatusCode.Unauthorized;

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.StatusCode.Should().Be(expected);
    }

    [Fact]
    public async Task GetCustomerStays_AccessingSecondRouteWithoutAuthorisation_ShouldThrowUnauthorisedException()
    {
        // Arrange
        var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
        var client = server.CreateClient();
        var url = "/api/customer/support-community";

        var expected = HttpStatusCode.Unauthorized;

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.StatusCode.Should().Be(expected);
    }
}
