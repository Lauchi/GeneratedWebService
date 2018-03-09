using System;
using Domain;

namespace Application
{
    public interface IDomainHook
    {
        Type EventType { get; }
        HookResult Execute(DomainEventBase domainEvent);
    }
}