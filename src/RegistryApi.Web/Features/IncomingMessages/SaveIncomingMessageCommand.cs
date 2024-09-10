namespace RegistryApi.Web.Features.IncomingMessages;

using RegistryApi.SharedKernel;
using MediatR;
using System;

public record SaveIncomingMessageCommand(
    string Event, int CustomerId, string Message, string Contact, DateTime Timestamp)
    : IRequest<Response<Unit>>;
