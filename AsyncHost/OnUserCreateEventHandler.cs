using System.Diagnostics;
using System.Threading.Tasks;
using Application;
using Application.Users;
using Domain.Users;
using Hangfire;

namespace AsyncHost
{
    public class OnUserCreateEventHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IRowVersionRepository _rowVersionRepository;
        public OnUserCreateEventAsynchronousHook AsyncHook { get; }

        public OnUserCreateEventHandler(IUserRepository userRepository, IEventStoreRepository eventStoreRepository, IRowVersionRepository rowVersionRepository, OnUserCreateEventAsynchronousHook asyncHook)
        {
            _userRepository = userRepository;
            _eventStoreRepository = eventStoreRepository;
            _rowVersionRepository = rowVersionRepository;
            AsyncHook = asyncHook;

            RecurringJob.AddOrUpdate(() => Run(), Cron.Minutely);
        }

        public async Task Run()
        {
            long lastRowVersion = _rowVersionRepository.GetVersion<UserCreateEvent>();
            var userCreateEvents = await _eventStoreRepository.GetEventsSince<UserCreateEvent>(lastRowVersion);
            foreach (var eve in userCreateEvents)
            {
                var createEvent = (UserCreateEvent) eve;
                var user = await _userRepository.GetUser(createEvent.Id);
                var userCreateEvent = new UserCreateEvent(user, createEvent.EntityId);
                var hookResult = AsyncHook.Execute(userCreateEvent);
                if (hookResult.Ok)
                {
                    _rowVersionRepository.SaveVersion<UserCreateEvent>(createEvent.CreatedAt);
                }
                else
                {
                    throw new AsyncHookBlockedException();
                }
            }
            var users = await _userRepository.GetUsers();
            Debug.WriteLine($"User Count: {users.Count}");
        }
    }
}