using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Users;

namespace Application.Users
{
    public interface IUserRepository
    {
        Task<User> GetUser(Guid id);
        Task UpdateUser(User parsedUser);
        Task CreateUser(User userEventUser);
        Task<IList<User>> GetUsers();
    }
}