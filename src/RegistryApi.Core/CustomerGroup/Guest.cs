namespace RegistryApi.Core.CustomerGroup;

using RegistryApi.SharedKernel;

public class Guest : IEntity
{
    public string? FirstName { get; private set; }
    public string? MiddleName { get; private set; }
    public string? LastName { get; private set; }
    public string? Honorific { get; private set; }
    public string? Phone { get; private set; }
    public bool PhoneIsPrimary { get; private set; }
    public string? Phone2 { get; private set; }
    public string? Phone2Description { get; private set; }
    public bool Phone2IsPrimary { get; private set; }
    public string? Email { get; private set; }
    public bool IsGuardian { get; private set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; private set; } = null!;
    private int? RelationshipId { get; set; }
    public bool IsPatient => RelationshipId == 1;
    public int Id { get; private set; }
}