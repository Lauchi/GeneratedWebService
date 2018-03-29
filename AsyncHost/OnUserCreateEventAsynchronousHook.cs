using System;
using Application;
using Domain.Users;

namespace AsyncHost
{
    public class OnUserCreateEventAsynchronousHook
    {
        public HookResult Execute(UserCreateEvent eve)
        {
            Console.WriteLine($"Neuer User Event abgearbeitet {eve.EntityId}");
            return HookResult.OkResult();
        }
    }
}