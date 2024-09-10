namespace RegistryApi.UnitTests.Core.User;

using RegistryApi.Core.Users;
using FluentAssertions;
using Xunit;

public class User_Create
{
    [Fact]
    public void CreateUser_WithValidData_ShouldSucceed()
    {
        // Arrange
        var username = "testuser";
        var password = "password";
        var customerId = 1;

        // Act
        var user = User.CreateNewUser(username, password, customerId);

        // Assert
        user.Should().NotBeNull();
        user.Username.Should().Be(username);
        user.Password.Should().Be(password);
        user.CustomerId.Should().Be(customerId);
    }
}
