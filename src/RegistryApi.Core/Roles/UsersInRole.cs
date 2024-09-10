namespace RegistryApi.Core.Roles;

using RegistryApi.Core.Users;

public class UsersInRole
{
    public int UserId { get; private set; } = default!;

    public User User { get; private set; } = default!;

    public int RoleId { get; private set; } = default!;

    public Role Role { get; private set; } = default!;
}
