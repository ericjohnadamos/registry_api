namespace RegistryApi.SharedKernel;

using System;
using MediatR;

public abstract class DomainEventBase : INotification
{
    protected DomainEventBase() => Id = Guid.NewGuid();

    public Guid Id { get; }
    public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
}