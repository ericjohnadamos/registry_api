namespace RegistryApi.Core.CustomerGroup;

using System.Collections.Generic;
using RegistryApi.SharedKernel;

public class Customer : IEntity
{
    private readonly List<Guest> _guests = [];
    private readonly List<CustomerLanguage> _languages = [];
    private readonly List<Stay> _stays = [];
    public string? Name { get; private set; }
    public bool ExcludeFromApi { get; private set; }
    public string? PrimaryPhone { get; private set; }
    public string? PrimaryPhoneDescription { get; private set; }

    public IReadOnlyCollection<Stay> Stays => _stays;
    public IReadOnlyCollection<Guest> Guests => _guests;
    public IReadOnlyCollection<CustomerLanguage> Languages => _languages;

    public int Id { get; private set; }
}
