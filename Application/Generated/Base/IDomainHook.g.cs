//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Application
{
    using System;
    using Domain;
    
    
    public interface IDomainHook
    {
        
        Type EventType
        {
            get;
        }
        
        HookResult ExecuteSavely(DomainEventBase domainEvent);
    }
}
