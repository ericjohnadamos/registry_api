namespace RegistryApi.Core.Customers;

using RegistryApi.SharedKernel;

public class Customer : IEntity
{
    private Customer()
    {
        Id = default!;
        Name = default!;
        Key = default!;
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Key { get; private set; }
    public string? TimeZoneId { get; private set; }
}
