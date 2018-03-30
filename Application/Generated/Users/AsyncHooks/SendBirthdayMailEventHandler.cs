using System.Collections.Generic;
using System.Threading.Tasks;
using Application;
using Domain;
using Domain.Users;

namespace AsyncHost
{
    public class SendBirthdayMailEventHandler
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        public SendBirthdayMailAsyncHook AsyncHook { get; }

        public SendBirthdayMailEventHandler(IEventStoreRepository eventStoreRepository, SendBirthdayMailAsyncHook asyncHook)
        {
            _eventStoreRepository = eventStoreRepository;
            AsyncHook = asyncHook;
        }

        public async Task Run()
        {
            var userCreateEvents = await _eventStoreRepository.GetEventsInQueue<UserUpdateNameEvent>();
            var handledEvents = new List<DomainEventBase>();
            foreach (var eve in userCreateEvents)
            {
                var updateEvent = (UserUpdateNameEvent) eve;
                var hookResult = AsyncHook.Execute(updateEvent);
                if (hookResult.Ok)
                {
                    handledEvents.Add(updateEvent);
                }
            }

            await _eventStoreRepository.RemoveEventsFromQueue(handledEvents);
        }
    }
}