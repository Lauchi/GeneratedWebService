using System;
using Application;
using Domain.Users;

namespace AsyncHost
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