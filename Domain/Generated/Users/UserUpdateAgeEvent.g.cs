//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Domain.Users
{
    using System;
    
    
    public class UserUpdateAgeEvent : DomainEventBase
    {
        
        public Int32 Age { get; private set; }
        
        private UserUpdateAgeEvent() : 
                base(Guid.Empty)
        {
        }
        
        public UserUpdateAgeEvent(Int32 Age, Guid EntityId) : 
                base(EntityId)
        {
            this.Age = Age;
        }
    }
}
