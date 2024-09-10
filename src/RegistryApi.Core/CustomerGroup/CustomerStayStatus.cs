using System;
using System.Linq;

namespace RegistryApi.Core.CustomerGroup;

public sealed class CustomerStayStatus
{
    public static readonly CustomerStayStatus CheckedIn = new CustomerStayStatus("CheckedIn");
    public static readonly CustomerStayStatus RequestList = new CustomerStayStatus("RequestList");
    public static readonly CustomerStayStatus WaitList = new CustomerStayStatus("WaitList");
    public static readonly CustomerStayStatus NoStatus = new CustomerStayStatus("NoStatus");
    public readonly string Name;
    private CustomerStayStatus(string name) => Name = name;
    public static string[] ListNames() => new[] {CheckedIn.Name, RequestList.Name, WaitList.Name, NoStatus.Name};

    public static CustomerStayStatus Create(string name)
    {
        var possibleValues = ListNames();
        if (!possibleValues.Contains(name))
            throw new ArgumentOutOfRangeException(nameof(name), $"{name} is not a valid value for status name.");
        return new CustomerStayStatus(name);
    }
}