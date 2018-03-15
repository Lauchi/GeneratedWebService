using System;
using System.Collections.Generic;
using Domain;
using Domain.Users;

namespace Application.Users.Hooks
{
    public partial class CreateUserEventHook
    {
        private HookResult Execute(UserCreateEvent userCreateEvent)
        {
            var newUserAge = userCreateEvent.User.Age + 10;
            var domainEventBases = new List<DomainEventBase>();
            domainEventBases.Add(new UserUpdateAgeEvent(newUserAge, Guid.NewGuid()));
            return HookResult.OkResult();
        }
    }
}