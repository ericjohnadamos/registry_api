using System.Collections.Generic;

namespace RegistryApi.SharedKernel;

public abstract class DomainEntityBase : IEntity
{
    public readonly Queue<DomainEventBase> DomainEvents = new Queue<DomainEventBase>();
    public int Id { get; private set; }
    protected void Raise(DomainEventBase domainEvent)
        => DomainEvents.Enqueue(domainEvent);
}