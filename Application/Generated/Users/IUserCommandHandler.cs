using System;
using System.Threading.Tasks;
using Application.Users.Commands;
using GeneratedWebService.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Application.Users
{
    public interface IUserCommandHandler
    {
        Task<IActionResult> CreateUser(CreateUserCommand createUserCommand);
        Task<IActionResult> UpdateUserName(Guid id, UpdateUserNameCommand updateUserNameCommand);
        Task<IActionResult> GetUser(Guid id);
        Task<IActionResult> GetUsers();
    }
}