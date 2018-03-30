using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncHost;
using Domain;
using Domain.Users;

namespace Application.Users.AsyncHooks
{
    public class SendWelcomeMailEventHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventStoreRepository _eventStoreRepository;
        public SendWelcomeMailAsyncHook AsyncHook { get; }

        public SendWelcomeMailEventHandler(IUserRepository userRepository, IEventStoreRepository eventStoreRepository, SendWelcomeMailAsyncHook asyncHook)
        {
            _userRepository = userRepository;
            _eventStoreRepository = eventStoreRepository;
            AsyncHook = asyncHook;
        }

        public async Task Run()
        {
            var userCreateEvents = await _eventStoreRepository.GetEventsInQueue<UserCreateEvent>();
            var handledEvents = new List<DomainEventBase>();
            foreach (var eve in userCreateEvents)
            {
                var createEvent = (UserCreateEvent) eve;
                var user = await _userRepository.GetUser(createEvent.Id);
                var userCreateEvent = new UserCreateEvent(user, createEvent.EntityId);
                var hookResult = AsyncHook.Execute(userCreateEvent);
                if (hookResult.Ok)
                {
                    handledEvents.Add(createEvent);
                }
            }

            await _eventStoreRepository.RemoveEventsFromQueue(handledEvents);
        }
    }
}