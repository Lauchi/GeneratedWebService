using System;
using Domain;
using Domain.Users;

namespace Application.Users.Hooks
{
    public partial class CreateUserEventHook : IDomainHook
    {
        public Type EventType { get; }

        public CreateUserEventHook()
        {
            EventType = typeof(UserCreateEvent);
        }

        public HookResult ExecuteSave(DomainEventBase domainEvent)
        {
            if (domainEvent is UserCreateEvent parsedEvent)
            {
                return Execute(parsedEvent);
            }
            throw new Exception("Event is not in the correct list");
        }
    }
}