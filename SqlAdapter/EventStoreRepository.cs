using System.Collections.Generic;
using System.Threading.Tasks;
using Application;
using Domain;

namespace SqlAdapter
{
    public class EventStoreRepository : IEventStoreRepository
    {
        private readonly EventStoreContext _context;

        public EventStoreRepository(EventStoreContext context)
        {
            _context = context;
        }
        public async Task AddEvents(List<DomainEventBase> domainEvents)
        {
            await _context.EventHistory.AddRangeAsync(domainEvents);
        }
    }
}