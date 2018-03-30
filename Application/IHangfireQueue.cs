using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Domain.Users;

namespace SqlAdapter
{
    public interface IHangfireQueue
    {
        Task AddEvents(List<DomainEventBase> domainEvents);
        Task<List<EventAndJob>> GetEvents(string jobName);
        Task RemoveEventsFromQueue(List<EventAndJob> handledEvents);
    }
}