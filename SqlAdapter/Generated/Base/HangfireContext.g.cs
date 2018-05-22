//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SqlAdapter
{
    using System;
    using Application;
    using Microsoft.EntityFrameworkCore;
    using Domain.Users;
    using Domain.Posts;
    
    
    public class HangfireContext : DbContext
    {
        
        public DbSet<EventAndJob> EventAndJobQueue { get; private set; }
        
        public DbSet<UserUpdateAgeEvent> UserUpdateAgeEvents { get; private set; }
        
        public DbSet<UserUpdateNameEvent> UserUpdateNameEvents { get; private set; }
        
        public DbSet<UserAddPostEvent> UserAddPostEvents { get; private set; }
        
        public DbSet<UserCreateEvent> UserCreateEvents { get; private set; }
        
        public DbSet<PostUpdateTitleEvent> PostUpdateTitleEvents { get; private set; }
        
        public DbSet<PostCreateEvent> PostCreateEvents { get; private set; }
        
        public HangfireContext(DbContextOptions<HangfireContext> options) : 
                base(options)
        {
        }
    }
}
