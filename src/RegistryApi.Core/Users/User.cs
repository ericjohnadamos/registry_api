namespace RegistryApi.Core.Users;

using RegistryApi.Core.Customers;
using RegistryApi.Core.Roles;
using RegistryApi.SharedKernel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class User : DomainEntityBase
{
    private User()
    {
    }

    private User(string username, string password, int customerId)
    {
        this.Username = username;
        this.Password = password;
        this.CustomerId = customerId;

        this.Raise(new UserCreated(this));
    }

    public string Username { get; private set; } = default!;

    public string Password { get; private set; } = default!;

    public int CustomerId { get; private set; } = default!;

    public Customer Customer { get; private set; } = default!;

    public ICollection<UsersInRole> UsersInRole { get; set; } = new Collection<UsersInRole>();

    public void SetPassword(string newPassword)
        => Password = newPassword;

    public static User CreateNewUser(string username, string password, int customerId)
        => new(username, password, customerId);
}