namespace RegistryApi.Core;

using System.Security.Claims;

public static class ApiUserClaimTypes
{
    public const string CustomerKey = @"CustomerKey";
    public const string TimeZoneId = @"TimeZoneId";
    public const string Username = ClaimTypes.Name;
    public const string Password = @"Password";
}