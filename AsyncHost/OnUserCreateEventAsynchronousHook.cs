using Application;
using Domain.Users;

namespace AsyncHost
{
    public class OnUserCreateEventAsynchronousHook
    {
        public HookResult Execute(UserCreateEvent eve)
        {
            return HookResult.OkResult();
        }
    }
}