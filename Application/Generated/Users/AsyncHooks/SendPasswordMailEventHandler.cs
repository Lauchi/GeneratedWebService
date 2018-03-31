using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Users;

namespace Application.Users.AsyncHooks
{
    public class SendPasswordMailEventHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IHangfireQueue _hangfireQueue;
        public SendPasswordMailAsyncHook AsyncHook { get; }

        public SendPasswordMailEventHandler(IUserRepository userRepository, IHangfireQueue hangfireQueue, SendPasswordMailAsyncHook asyncHook)
        {
            _userRepository = userRepository;
            _hangfireQueue = hangfireQueue;
            AsyncHook = asyncHook;
        }

        public async Task Run()
        {
            var userCreateEvents = await _hangfireQueue.GetEvents("SendPasswordMail");
            var handledEvents = new List<EventAndJob>();
            foreach (var eve in userCreateEvents)
            {
                var createEvent = (UserCreateEvent) eve.DomainEvent;
                var user = await _userRepository.GetUser(createEvent.Id);
                var userCreateEvent = new UserCreateEvent(user, createEvent.EntityId);
                var hookResult = AsyncHook.Execute(userCreateEvent);
                if (hookResult.Ok)
                {
                    handledEvents.Add(eve);
                }
            }

            await _hangfireQueue.RemoveEventsFromQueue(handledEvents);
        }
    }
}