using System;
using System.Collections.Generic;
using Domain;
using Domain.Users;

namespace Application.Users.Hooks
{
    public partial class CreateUserEventHook
    {
        public HookResult Execute(DomainEventBase domainEvent)
        {
            if (domainEvent is UserCreateEvent parsedEvent)
            {
                var newUserAge = parsedEvent.User.Age + 10;
                var domainEventBases = new List<DomainEventBase>();
                domainEventBases.Add(new UserUpdateAgeEvent(newUserAge, Guid.NewGuid()));
                return HookResult.OkResult();
            }
            return HookResult.ErrorResult(new List<string> {"Irgend ein fehler"});
        }
    }
}