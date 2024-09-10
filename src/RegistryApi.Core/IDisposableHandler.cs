using System;
using MediatR;

namespace RegistryApi.Core
{
	public interface IDisposableHandler<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse>, IDisposable
		where TRequest : IRequest<TResponse>
	{
	}
}