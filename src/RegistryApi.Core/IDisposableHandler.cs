namespace RegistryApi.Core;

using System;
using MediatR;

public interface IDisposableHandler<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse>, IDisposable
	where TRequest : IRequest<TResponse>
{
}