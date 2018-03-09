using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Application.Users.Hooks;
using Domain;

namespace SqlAdapter
{
    public class EventStore : IEventStore
    {
        private readonly EventStoreContext _context;

        public EventStore(EventStoreContext context)
        {
            _context = context;
            DomainHooks = new List<IDomainHook> {new CreateUserEventHook()};
        }

        public IEnumerable<IDomainHook> DomainHooks { get; }

        public async Task<HookResult> AppendAll(List<DomainEventBase> domainEvents)
        {
            foreach (var domainEvent in domainEvents)
            {
                var domainHooks = DomainHooks.Where(hook => hook.EventType == domainEvent.GetType());
                foreach (var domainHook in domainHooks)
                {
                    var validationResult = domainHook.Execute(domainEvent);
                    if (!validationResult.Ok)
                        return validationResult;
                }
            }

            _context.EventHistory.AddRange(domainEvents);
            await _context.SaveChangesAsync();
            return HookResult.OkResult();
        }
    }
}