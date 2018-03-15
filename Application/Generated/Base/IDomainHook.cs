using System;
using Domain;

namespace Application
{
    public interface IDomainHook
    {
        Type EventType { get; }
        HookResult ExecuteSave(DomainEventBase domainEvent);
    }
}