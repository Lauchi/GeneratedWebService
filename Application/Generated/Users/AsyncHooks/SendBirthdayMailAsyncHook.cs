using System;
using Application;
using Domain.Users;

namespace Application.Users.AsyncHooks
{
    public class SendBirthdayMailAsyncHook
    {
        public HookResult Execute(UserUpdateNameEvent eve)
        {
            Console.WriteLine($"Neuer User Event abgearbeitet {eve.EntityId}");
            return HookResult.OkResult();
        }
    }
}