namespace RegistryApi.Core.Users;

public class UserInfo
{
    public UserInfo(int userId, string username, string customerKey, string timeZoneId)
    {
        UserId = userId;
        Username = username;
        CustomerKey = customerKey;
        TimeZoneId = timeZoneId;
    }

    public int UserId { get; }
    public string Username { get; }
    public string CustomerKey { get; }
    public string TimeZoneId { get; }
}