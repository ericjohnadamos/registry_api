using System;
using MediatR;

namespace RegistryApi.SharedKernel;

public abstract class DomainEventBase : INotification
{
    protected DomainEventBase() => Id = Guid.NewGuid();

    public Guid Id { get; }
    public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
}