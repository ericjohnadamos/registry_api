namespace RegistryApi.Core.CustomerGroup;

using RegistryApi.Core.Shared;

public class CustomerLanguage
{
    private CustomerLanguage()
    {
        CustomerId = default!;
        Customer = default!;
        Language = default!;
        LanguageId = default!;
    }
    public int CustomerId { get; private set; }
    public Customer Customer { get; private set; }
    public int LanguageId { get; private set; }
    public Language Language { get; set; }
}