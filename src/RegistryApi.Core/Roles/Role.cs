namespace RegistryApi.Core.Roles;

using RegistryApi.SharedKernel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class Role : IEntity
{
    private Role()
    {
    }

    public int Id { get; private set; } = default!;

    public int ApplicationId { get; private set; } = default!;

    public string Name { get; private set; } = default!;

    public ICollection<UsersInRole> UsersInRole { get; set; } = new Collection<UsersInRole>();
}
