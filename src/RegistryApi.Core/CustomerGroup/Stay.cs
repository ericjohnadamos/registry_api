namespace RegistryApi.Core.CustomerGroup;

using System;
using RegistryApi.SharedKernel;

public class Stay : IEntity
{
    public int? CustomerId { get; set; }
    public DateTime? WaitListDateTime { get; private set; }
    public DateTime? CancelledDateTime { get; private set; }
    public DateTime? RequestedCheckIn { get; private set; }
    public DateTime? CheckInDateTime { get; private set; }
    public DateTime? CheckOutDateTime { get; private set; }
    public DateTime? ExpectedCheckOut { get; private set; }
    public Customer Customer { get; private set; } = null!;
    public int Id { get; private set; }

    public CustomerStayStatus Status(DateTime today)
    {
        if (CheckInDateTime.HasValue && !CheckOutDateTime.HasValue)
            return CustomerStayStatus.CheckedIn;
        if (   (WaitListDateTime.HasValue && WaitListDateTime.Value.Date <= today)
            && (ExpectedCheckOut == null || ExpectedCheckOut.Value.Date >= today))
            return CustomerStayStatus.WaitList;
        if (   WaitListDateTime == null
            || WaitListDateTime != null
            && WaitListDateTime.Value.Date >= today
            && (ExpectedCheckOut == null || ExpectedCheckOut.Value.Date >= today))
            return CustomerStayStatus.RequestList;
        return CustomerStayStatus.NoStatus;
    }
}