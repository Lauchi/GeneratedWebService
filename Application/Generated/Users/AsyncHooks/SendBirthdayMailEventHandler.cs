using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Domain.Users;
using SqlAdapter;

namespace AsyncHost
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
                var updateEvent = (UserUpdateNameEvent) eve.DomainEvent;
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