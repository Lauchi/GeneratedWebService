using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Users;

namespace Application.Users.AsyncHooks
{
    public class SendBirthdayMailEventHandler
    {
        private readonly IHangfireQueue _hangfireQueue;
        public SendBirthdayMailAsyncHook AsyncHook { get; }

        public SendBirthdayMailEventHandler(IHangfireQueue hangfireQueue, SendBirthdayMailAsyncHook asyncHook)
        {
            _hangfireQueue = hangfireQueue;
            AsyncHook = asyncHook;
        }

        public async Task Run()
        {
            var userCreateEvents = await _hangfireQueue.GetEvents("SendBirthdayMail");
            var handledEvents = new List<EventAndJob>();
            foreach (var eve in userCreateEvents)
            {
                var updateEvent = (UserUpdateAgeEvent) eve.DomainEvent;
                var hookResult = AsyncHook.Execute(updateEvent);
                if (hookResult.Ok)
                {
                    handledEvents.Add(eve);
                }
            }

            await _hangfireQueue.RemoveEventsFromQueue(handledEvents);
        }
    }
}