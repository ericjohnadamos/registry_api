namespace RegistryApi.Web.Roles;

using System.Collections.Generic;

public static class RolePolicyMapping
{
    static RolePolicyMapping()
    {
        Mapping = new Dictionary<string, List<string>>
        {
            { UserTypePolicies.Security, new List<string>() { UserTypePolicies.Security } },
            { UserTypePolicies.Staff, new List<string>() { UserTypePolicies.Staff } },
            { UserTypePolicies.Admin, new List<string>() { UserTypePolicies.Admin } },
            { UserTypePolicies.Edit, new List<string>() { UserTypePolicies.Edit } },
            { UserTypePolicies.Master, new List<string>() { UserTypePolicies.Master } },
            { UserTypePolicies.AdministrativeContact, new List<string>() { UserTypePolicies.AdministrativeContact } },
            { UserTypePolicies.SpecialFileAccess, new List<string>() { UserTypePolicies.SpecialFileAccess } },
            { UserTypePolicies.EditStayDates, new List<string>() { UserTypePolicies.EditStayDates } },
            { UserTypePolicies.EditReservations, new List<string>() { UserTypePolicies.EditReservations } },
            { UserTypePolicies.AccessReservations, new List<string>() { UserTypePolicies.AccessReservations } },
            { UserTypePolicies.EditCheckIn, new List<string>() { UserTypePolicies.EditCheckIn } },
            { UserTypePolicies.CustomerDashboard, new List<string>() { UserTypePolicies.CustomerDashboard } },
            { UserTypePolicies.CustomerFamily, new List<string>() { UserTypePolicies.CustomerFamily } },
            { UserTypePolicies.CustomerGuests, new List<string>() { UserTypePolicies.CustomerGuests } },
            { UserTypePolicies.CustomerStays, new List<string>() { UserTypePolicies.CustomerStays } },
            { UserTypePolicies.CustomerPayments, new List<string>() { UserTypePolicies.CustomerPayments } },
            { UserTypePolicies.CustomerInvoices, new List<string>() { UserTypePolicies.CustomerInvoices } },
            { UserTypePolicies.CustomerItems, new List<string>() { UserTypePolicies.CustomerItems } },
            { UserTypePolicies.CustomerNotes, new List<string>() { UserTypePolicies.CustomerNotes } },
            { UserTypePolicies.CustomerFiles, new List<string>() { UserTypePolicies.CustomerFiles } },
            { UserTypePolicies.RoomsEditRoomProperties, new List<string>() { UserTypePolicies.RoomsEditRoomProperties } },
            { UserTypePolicies.BackgroundChecks, new List<string>() { UserTypePolicies.BackgroundChecks } },
            { UserTypePolicies.CustomerVaccinations, new List<string>() { UserTypePolicies.CustomerVaccinations } },
            { UserTypePolicies.CustomerSummary, new List<string>() { UserTypePolicies.CustomerSummary } },
            { UserTypePolicies.RegistrationImport, new List<string>() { UserTypePolicies.RegistrationImport } },
            { UserTypePolicies.CustomerGroupDayUse, new List<string>() { UserTypePolicies.CustomerGroupDayUse } },
        };
    }

    public static Dictionary<string, List<string>> Mapping { get; set; } = null!;
}
