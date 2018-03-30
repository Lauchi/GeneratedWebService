using System;
using Application;
using Domain.Users;

namespace Application.Users.AsyncHooks
{
    public class SendWelcomeMailAsyncHook
    {
        public HookResult Execute(UserCreateEvent eve)
        {
            Console.WriteLine($"Neuer User Event abgearbeitet {eve.EntityId}");
            return HookResult.OkResult();
        }
    }
}