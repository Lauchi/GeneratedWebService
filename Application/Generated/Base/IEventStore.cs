using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace Application
{
    public interface IEventStore
    {
        Task<HookResult> AppendAll(List<DomainEventBase> domainEvents);
    }
}