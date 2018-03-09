using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Users.Commands;
using Domain.Users;
using Microsoft.AspNetCore.Mvc;

namespace Application.Users
{
    public class UserCommandHandler : IUserCommandHandler
    {
        private readonly IEventStore _eventStore;
        private readonly IUserRepository _userRepository;

        public UserCommandHandler(IEventStore eventStore, IUserRepository userRepository)
        {
            _eventStore = eventStore;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> CreateUser(UserCreateCommand createUserCommand)
        {
            var createUserResult = User.Create(createUserCommand.Name);
            if (createUserResult.Ok)
            {
                var hookResult = await _eventStore.AppendAll(createUserResult.DomainEvents);
                if (hookResult.Ok)
                {
                    await _userRepository.CreateUser(createUserResult.CreatedEntity);
                    return new CreatedResult("uri", createUserResult.CreatedEntity);
                }

                return new BadRequestObjectResult(hookResult.Errors);
            }

            return new BadRequestObjectResult(createUserResult.DomainErrors);
        }

        public async Task<IActionResult> UpdateUserName(Guid id, UserUpdateNameCommand updateUserNameCommand)
        {
            var user = await _userRepository.GetUser(id);
            if (user != null)
            {
                var validationResult = user.UpdateName(updateUserNameCommand.Name);
                if (validationResult.Ok)
                {
                    var hookResult = await _eventStore.AppendAll(validationResult.DomainEvents);
                    if (hookResult.Ok)
                    {
                        await _userRepository.UpdateUser(user);
                        return new OkResult();
                    }

                    return new BadRequestObjectResult(hookResult.Errors);
                }

                return new BadRequestObjectResult(validationResult.DomainErrors);
            }

            return new NotFoundObjectResult(new List<string> { $"Could not find User with ID: {id}"});
        }

        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _userRepository.GetUser(id);
            if (user != null) return new JsonResult(user);

            return new NotFoundObjectResult(new List<string> { $"Could not find User with ID: {id}" });
        }

        public async Task<IActionResult> GetUsers()
        {
            var users = await _userRepository.GetUsers();
            return new OkObjectResult(users);
        }
    }
}