namespace RegistryApi.Web.Features.Customers;

using RegistryApi.Core;
using RegistryApi.Core.CustomerGroup;
using RegistryApi.Core.CustomerGroup.Specifications;
using RegistryApi.Core.Shared;
using RegistryApi.Infrastructure.Data;
using RegistryApi.Web.Endpoints.SupportCommunity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class GetCustomerStaysQueryHandler
    : IDisposableHandler<GetCustomerStaysQuery, GetCustomerStaysResponse>
{
    private readonly StaysReadRepository staysReadRepository;
    private readonly GlobalRegistryContext globalRegistryContext;
    private readonly ISystemClock systemClock;

    public GetCustomerStaysQueryHandler(
        StaysReadRepository staysReadRepository,
        GlobalRegistryContext globalRegistryContext,
        ISystemClock systemClock)
    {
        Contract.Assert(staysReadRepository != null);
        Contract.Assert(globalRegistryContext != null);
        Contract.Assert(systemClock != null);

        this.staysReadRepository = staysReadRepository;
        this.globalRegistryContext = globalRegistryContext;
        this.systemClock = systemClock;
    }

    public void Dispose() => this.staysReadRepository.Dispose();

    public async Task<GetCustomerStaysResponse> Handle(
        GetCustomerStaysQuery query, CancellationToken cancellationToken)
    {
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById(query.UserInfo.TimeZoneId);
        var today = TimeZoneInfo.ConvertTimeFromUtc(this.systemClock.UtcNow.UtcDateTime, timeZone).Date;
        var accountOptions = await this.staysReadRepository.GetAccountOptions();

        var specification
            = new StayStatusSpecification(today, accountOptions.ShouldDropOldRequestsOffWaitList).ToExpression();
        var stays = await this.staysReadRepository.Query(query.User).Where(specification).ToListAsync();

        var customerStays = stays.GroupBy(_ => _.Customer);
        var customers = new List<CustomerDto>();

        foreach (var stay in customerStays)
        {
            // check for check-in record. if present, add to result
            // else if wait list record if present, add to result
            // else add request list
            var checkedIn = stay.FirstOrDefault(_ => _.Status(today) == CustomerStayStatus.CheckedIn);
            var waitList = stay.FirstOrDefault(_ => _.Status(today) == CustomerStayStatus.WaitList);
            var requestList = stay.FirstOrDefault(_ => _.Status(today) == CustomerStayStatus.RequestList);

            CustomerDto customerDto;

            if (checkedIn != null)
                customerDto = CreateCustomerDto(checkedIn, today);
            else if (waitList != null)
                customerDto = CreateCustomerDto(waitList, today);
            else if (requestList != null)
                customerDto = CreateCustomerDto(requestList, today);
            else
                customerDto = CreateCustomerDto(stay.First(), today);

            if (!customers.Contains(customerDto)) customers.Add(customerDto);
        }

        return new GetCustomerStaysResponse(customers.OrderBy(_ => _.Name));
    }

    private CustomerDto CreateCustomerDto(Stay stay, DateTime today)
    {
        var stayLanguageIds = stay.Customer.Languages.Select(l => l.LanguageId).ToList();
        var languages = this.globalRegistryContext.Language
            .AsNoTracking()
            .Where(language => stayLanguageIds.Contains(language.Id))
            .ToList();
        var dto = new CustomerDto
        {
            Id = stay.Customer.Id,
            Name = stay.Customer.Name,
            Status = stay.Status(today).Name,
            RequestedCheckIn = stay.RequestedCheckIn?.ToString("yyyy-MM-dd"),
            CheckIn = stay.CheckInDateTime?.ToString("yyyy-MM-dd"),
            ExpectedCheckOut = stay.ExpectedCheckOut?.ToString("yyyy-MM-dd"),
            Languages = languages,
        };

        if (!string.IsNullOrWhiteSpace(stay.Customer.PrimaryPhone))
            dto.PrimaryPhoneNumbers.Add(new PhoneNumberInfo(stay.Customer.PrimaryPhone, "Customer Primary Phone"));

        if (!stay.Customer.Guests.Any())
            return dto;

        dto.Guests = new List<GuestDto>();
        foreach (var guest in stay.Customer.Guests)
        {
            dto.Guests.Add(new GuestDto
            {
                Phone = guest.Phone,
                Phone2 = !string.IsNullOrWhiteSpace(guest.Phone2) ? new PhoneNumberInfo(guest.Phone2, $"{guest.FirstName} {guest.LastName}") : default,
                Email = guest.Email,
                FirstName = guest.FirstName,
                Honorific = guest.Honorific,
                LastName = guest.LastName,
                MiddleName = guest.MiddleName,
                Id = guest.Id,
                IsGuardian = guest.IsGuardian,
                IsPatient = guest.IsPatient
            });

            if (guest.PhoneIsPrimary && !string.IsNullOrWhiteSpace(guest.Phone))
                dto.PrimaryPhoneNumbers.Add(new PhoneNumberInfo(guest.Phone, $"{guest.FirstName} {guest.LastName}"));
            if (guest.Phone2IsPrimary && !string.IsNullOrWhiteSpace(guest.Phone2))
                dto.PrimaryPhoneNumbers.Add(new PhoneNumberInfo(guest.Phone2, $"{guest.FirstName} {guest.LastName}"));
        }

        return dto;
    }
}
