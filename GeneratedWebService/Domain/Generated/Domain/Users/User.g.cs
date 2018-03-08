//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using Domain.Posts;

namespace Domain.Users
{
    using System;
    
    
    public interface IUser
    {
        
        ValidationResult UpdateAge(Int32 Age);
        
        ValidationResult UpdateName(String Name);
    }
    
    public partial class User : IUser
    {

        public Guid Id { get; private set; }

        public String Name { get; private set; }

        public Int32 Age { get; private set; }

        public List<Post> Posts { get; private set; }

        private User(Guid Id, String Name, Int32 Age)
        {
            this.Name = Name;
            this.Age = Age;
        }
        
        private User()
        {
        }
    }
}
