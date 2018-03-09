using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Users;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace SqlAdapter.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly EventStoreContext _eventStore;

        public UserRepository(EventStoreContext eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<User> GetUser(Guid id)
        {
            return await _eventStore.Users.FindAsync(id);
        }

        public async Task UpdateUser(User user)
        {
            _eventStore.Users.Update(user);
            await _eventStore.SaveChangesAsync();
        }

        public async Task CreateUser(User user)
        {
            _eventStore.Users.Add(user);
            await _eventStore.SaveChangesAsync();
        }

        public async Task<IList<User>> GetUsers()
        {
            return await _eventStore.Users.ToListAsync();
        }
    }
}