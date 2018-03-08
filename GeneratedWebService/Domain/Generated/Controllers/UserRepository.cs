using System;
using System.Threading.Tasks;
using Domain.Users;
using GenericWebservice.Domain;
using Microsoft.EntityFrameworkCore;

namespace GeneratedWebService.Controllers
{
    public class UserRepository : IUserRepository
    {
        private readonly DbContextOptions<EventStoreContext> _options;

        public UserRepository(DbContextOptions<EventStoreContext> options)
        {
            _options = options;
        }
        public async Task<User> GetUser(Guid id)
        {
            using (var aggregateStore = new EventStoreContext(_options))
            {
                return await aggregateStore.Users.FindAsync(id);
            }
        }

        public async Task UpdateUser(User user)
        {
            using (var aggregateStore = new EventStoreContext(_options))
            {
                aggregateStore.Users.Update(user);
                await aggregateStore.SaveChangesAsync();
            }
        }

        public async Task CreateUser(User user)
        {
            using (var aggregateStore = new EventStoreContext(_options))
            {
                aggregateStore.Users.Add(user);
                await aggregateStore.SaveChangesAsync();
            }
        }
    }

    public interface IUserRepository
    {
        Task<User> GetUser(Guid id);
        Task UpdateUser(User parsedUser);
        Task CreateUser(User userEventUser);
    }
}