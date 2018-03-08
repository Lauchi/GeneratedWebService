using Domain;
using Domain.Posts;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace GenericWebservice.Domain
{
    public class EventStoreContext : DbContext
    {

        public EventStoreContext(DbContextOptions<EventStoreContext> options)
            : base(options)
        {
        }

        public DbSet<UserUpdateAgeEvent> UserUpdateAgeEvents { get; set; }
        public DbSet<UserUpdateNameEvent> UserUpdateNameEvents { get; set; }
        public DbSet<CreateUserEvent> CreateUserEvents { get; set; }
        public DbSet<CreatePostEvent> CreatePostEvents { get; set; }
        public DbSet<DomainEventBase> EventHistory { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
    }
}