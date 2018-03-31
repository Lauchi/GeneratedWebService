using System;
using Application;
using Domain.Users;

namespace Application.Users.AsyncHooks
{
    public class SendBirthdayMailAsyncHook
    {
        public HookResult Execute(UserUpdateAgeEvent eve)
        {
            Console.WriteLine($"Birthday Mail abgearbeitet {eve.EntityId}");
            return HookResult.OkResult();
        }
    }
}