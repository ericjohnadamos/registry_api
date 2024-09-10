using RegistryApi.SharedKernel;

namespace RegistryApi.Core.Users;

public class UserCreated : DomainEventBase
{
    public UserCreated(User newUser)
        => NewUser = newUser;

    public User NewUser { get; }
}