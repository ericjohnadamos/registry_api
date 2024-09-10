namespace RegistryApi.Core.Shared;

public class PhoneNumberInfo
{
    public PhoneNumberInfo(string? number, string? description)
    {
        Number = number;
        Description = description;
    }

    public PhoneNumberInfo(string number)
        : this(number, default)
    {
    }

    public string? Number { get; }
    public string? Description { get; }
}