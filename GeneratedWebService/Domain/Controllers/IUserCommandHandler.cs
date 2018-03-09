using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GeneratedWebService.Controllers
{
    public interface IUserCommandHandler
    {
        Task<IActionResult> CreateUser(CreateUserCommand createUserCommand);
        Task<IActionResult> UpdateUserName(Guid id, UpdateUserNameCommand updateUserNameCommand);
        Task<IActionResult> GetUser(Guid id);
        Task<IActionResult> GetUsers();
    }
}