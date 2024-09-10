namespace RegistryApi.Web.Features.Customers;

using RegistryApi.Core.Users;
using MediatR;
using System.Security.Claims;

public record GetCustomerStaysQuery(UserInfo UserInfo, ClaimsPrincipal User) : IRequest<GetCustomerStaysResponse>;
