using System.Diagnostics;
using System.Threading.Tasks;
using Application;
using Application.Users;
using Domain.Users;

namespace AsyncHost
{
    internal class OnUserCreate
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IRowVersionRepository _rowVersionRepository;

        public OnUserCreate(IUserRepository userRepository, IEventStoreRepository eventStoreRepository, IRowVersionRepository rowVersionRepository, OnUserCreateEventAsynchronousHook asyncHook)
        {
            _userRepository = userRepository;
            _eventStoreRepository = eventStoreRepository;
            _rowVersionRepository = rowVersionRepository;
            AsyncHook = asyncHook;
        }

        public async Task Run()
        {
            long lastRowVersion = _rowVersionRepository.GetUserCreateVersion();
            var userCreateEvents = _eventStoreRepository.GetUserCreateEvents(lastRowVersion);
            foreach (var eve in userCreateEvents)
            {
                var user = await _userRepository.GetUser(eve.Id);
                var userCreateEvent = new UserCreateEvent(user, eve.EntityId);
                var hookResult = AsyncHook.Execute(userCreateEvent);
                if (!hookResult.Ok)
                {
                    throw new AsyncHookBlockedException();
                }
            }
            var users = await _userRepository.GetUsers();
            Debug.WriteLine($"USer Count: {users.Count}");
        }

        public OnUserCreateEventAsynchronousHook AsyncHook { get; }
    }
}