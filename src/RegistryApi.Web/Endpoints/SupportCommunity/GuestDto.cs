namespace RegistryApi.Web.Endpoints.SupportCommunity;

using RegistryApi.Core.Shared;

public class GuestDto
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? Honorific { get; set; }
    public string? Phone { get; set; }
    public PhoneNumberInfo? Phone2 { get; set; }
    public string? Email { get; set; }
    public bool IsGuardian { get; set; }
    public bool IsPatient { get; set; }
}
