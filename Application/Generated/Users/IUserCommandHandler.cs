using System;
using System.Threading.Tasks;
using Application.Users.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Application.Users
{
    public interface IUserCommandHandler
    {
        Task<IActionResult> CreateUser(UserCreateCommand createUserCommand);
        Task<IActionResult> UpdateUserName(Guid id, UserUpdateNameCommand updateUserNameCommand);
        Task<IActionResult> GetUser(Guid id);
        Task<IActionResult> GetUsers();
    }
}