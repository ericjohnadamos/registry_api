using System;
using System.Linq.Expressions;
using RegistryApi.SharedKernel;

namespace RegistryApi.Core.CustomerGroup.Specifications;

public class StayStatusSpecification : Specification<Stay>
{
    private readonly bool _shouldDropOldRequestsOffWaitList;
    private readonly DateTime _today;

    public StayStatusSpecification(DateTime today, bool shouldDropOldRequestsOffWaitList)
    {
        _today = today;
        _shouldDropOldRequestsOffWaitList = shouldDropOldRequestsOffWaitList;
    }

    public override Expression<Func<Stay, bool>> ToExpression()
        => stay => stay.CheckOutDateTime == null && stay.CancelledDateTime == null && !stay.Customer.ExcludeFromApi
                   && (!_shouldDropOldRequestsOffWaitList || stay.CheckInDateTime != null || stay.ExpectedCheckOut == null || stay.ExpectedCheckOut >= _today);
}