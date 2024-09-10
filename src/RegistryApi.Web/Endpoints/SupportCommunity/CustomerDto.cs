namespace RegistryApi.Web.Endpoints.SupportCommunity;

using RegistryApi.Core.Shared;
using System;
using System.Collections.Generic;

public class CustomerDto : IEquatable<CustomerDto>
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Status { get; set; }
    public string? RequestedCheckIn { get; set; }
    public string? CheckIn { get; set; }
    public string? ExpectedCheckOut { get; set; }

    public List<GuestDto>? Guests { get; set; }
    public List<PhoneNumberInfo> PrimaryPhoneNumbers { get; set; } = new List<PhoneNumberInfo>();
    public List<Language> Languages { get; set; } = new List<Language>();

    public bool Equals(CustomerDto? other) => other != null && Id == other.Id && Name == other.Name;
}
