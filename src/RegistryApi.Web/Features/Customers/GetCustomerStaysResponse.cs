namespace RegistryApi.Web.Features.Customers;

using RegistryApi.Web.Endpoints.SupportCommunity;
using System.Collections.Generic;

public record GetCustomerStaysResponse(IEnumerable<CustomerDto> Customers);
