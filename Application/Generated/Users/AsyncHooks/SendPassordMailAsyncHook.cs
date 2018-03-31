using System;
using Application;
using Domain.Users;

namespace Application.Users.AsyncHooks
{
    public class SendPasswordMailAsyncHook
    {
        public HookResult Execute(UserCreateEvent eve)
        {
            Console.WriteLine($"Password Mail Event abgearbeitet {eve.EntityId}");
            return HookResult.OkResult();
        }
    }
}