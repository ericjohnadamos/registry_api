namespace RegistryApi.Core.CustomerGroup.Specifications;

using System;
using System.Linq.Expressions;
using RegistryApi.SharedKernel;

public class StayStatusSpecification(DateTime today, bool shouldDropOldRequestsOffWaitList) : Specification<Stay>
{
    private readonly bool _shouldDropOldRequestsOffWaitList = shouldDropOldRequestsOffWaitList;
    private readonly DateTime _today = today;

    public override Expression<Func<Stay, bool>> ToExpression()
        => stay => (   (   stay.CheckOutDateTime == null
                        && stay.CancelledDateTime == null
                        && !stay.Customer.ExcludeFromApi)
                    && (   !_shouldDropOldRequestsOffWaitList
                        || stay.CheckInDateTime != null
                        || stay.ExpectedCheckOut == null
                        || stay.ExpectedCheckOut >= _today));
}