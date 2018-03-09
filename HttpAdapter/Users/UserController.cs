using System;
using System.Threading.Tasks;
using Application.Users;
using Application.Users.Commands;
using Microsoft.AspNetCore.Mvc;

namespace HttpAdapter.Users
{
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly IUserCommandHandler _commandHandler;

        public UserController(IUserCommandHandler commandHandler)
        {
            _commandHandler = commandHandler;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return await _commandHandler.GetUser(id);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await _commandHandler.GetUsers();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateCommand createUserCommand)
        {
            return await _commandHandler.CreateUser(createUserCommand);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserName(Guid id, [FromBody] UserUpdateNameCommand updateUserNameCommand)
        {
            return await _commandHandler.UpdateUserName(id, updateUserNameCommand);
        }
    }
}