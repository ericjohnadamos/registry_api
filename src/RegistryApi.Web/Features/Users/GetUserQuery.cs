namespace RegistryApi.Web.Features.Users;

using RegistryApi.Core.Users;
using MediatR;
using Optional;
using System;

public record GetUserQuery(string Username, string Password) : IRequest<Option<User, Exception>>;
