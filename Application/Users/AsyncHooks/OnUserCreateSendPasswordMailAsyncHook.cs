//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Application.Users.AsyncHooks
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Users;
    
    
    public class OnUserCreateSendPasswordMailAsyncHook
    {
        
        public async Task<HookResult> Execute(UserCreateEvent domainEvent)
        {
            Console.WriteLine("OnUserCreateSendPasswordMailAsyncHook called");
            return await Task.FromResult(HookResult.OkResult());
        }
    }
}
