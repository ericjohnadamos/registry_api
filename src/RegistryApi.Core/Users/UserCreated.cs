using RegistryApi.SharedKernel;

namespace RegistryApi.Core.Users;

public class UserCreated(User newUser) : DomainEventBase
{
    public User NewUser { get; } = newUser;
}