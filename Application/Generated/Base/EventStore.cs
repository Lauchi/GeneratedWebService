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

        public EventStore(IEventStoreRepository eventRepository, CreateUserEventHook CreateUserEventHook)
        {
            _eventRepository = eventRepository;
            DomainHooks = new List<IDomainHook>();
            DomainHooks.Add(CreateUserEventHook);
        }

        public IList<IDomainHook> DomainHooks { get; }

        public async Task<HookResult> AppendAll(List<DomainEventBase> domainEvents)
        {
            foreach (var domainEvent in domainEvents)
            {
                var domainHooks = DomainHooks.Where(hook => hook.EventType == domainEvent.GetType());
                foreach (var domainHook in domainHooks)
                {
                    var validationResult = domainHook.ExecuteSave(domainEvent);
                    if (!validationResult.Ok)
                        return validationResult;
                }
            }

            await _eventRepository.AddEvents(domainEvents);
            return HookResult.OkResult();
        }
    }
}