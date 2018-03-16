using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Users.Hooks;
using Domain;

namespace Application
{
    public class EventStore
    {
        private readonly IEventStoreRepository _eventRepository;

        public EventStore(IEventStoreRepository eventRepository, SendPasswordMailHook SendPasswordMailHook)
        {
            _eventRepository = eventRepository;
            DomainHooks.Add(SendPasswordMailHook);
        }

        public IList<IDomainHook> DomainHooks { get; } = new List<IDomainHook>();

        public async Task<HookResult> AppendAll(List<DomainEventBase> domainEvents)
        {
            foreach (var domainEvent in domainEvents)
            {
                var domainHooks = DomainHooks.Where(hook => hook.EventType == domainEvent.GetType());
                foreach (var domainHook in domainHooks)
                {
                    var validationResult = domainHook.ExecuteSavely(domainEvent);
                    if (!validationResult.Ok)
                        return validationResult;
                }
            }

            await _eventRepository.AddEvents(domainEvents);
            return HookResult.OkResult();
        }
    }
}