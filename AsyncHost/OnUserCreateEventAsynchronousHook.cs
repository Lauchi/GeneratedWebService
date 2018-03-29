using Application;
using Domain.Users;

namespace AsyncHost
{
    internal class OnUserCreateEventAsynchronousHook
    {
        public HookResult Execute(UserCreateEvent eve)
        {
            return HookResult.OkResult();
        }
    }
}