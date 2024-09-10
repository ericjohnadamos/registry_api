namespace RegistryApi.Core.Shared;

using RegistryApi.SharedKernel;

public class Language : IEntity
{
    public int Id { get; private set; }
    public string? Name { get; private set; }
    public string? Abbreviation { get; private set; }
}
