using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Users.Hooks;
using Domain;

namespace Application
{
    public class EventStore : IEventStore
    {
        private readonly IEventStoreRepository _eventRepository;

        public EventStore(IEventStoreRepository eventRepository)
        {
            _eventRepository = eventRepository;
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

            await _eventRepository.AddEvents(domainEvents);
            return HookResult.OkResult();
        }
    }
}