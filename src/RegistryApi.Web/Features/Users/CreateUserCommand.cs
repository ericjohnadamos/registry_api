namespace RegistryApi.Web.Features.Users;

using RegistryApi.SharedKernel;
using MediatR;

public record CreateUserCommand(string Username, string Password, int CustomerId) : IRequest<Response<Unit>>;
