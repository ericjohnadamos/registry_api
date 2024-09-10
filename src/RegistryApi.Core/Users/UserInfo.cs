namespace RegistryApi.Core.Users;

public class UserInfo(int userId, string username, string customerKey, string timeZoneId)
{
    public int UserId { get; } = userId;
    public string Username { get; } = username;
    public string CustomerKey { get; } = customerKey;
    public string TimeZoneId { get; } = timeZoneId;
}