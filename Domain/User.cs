using System;
using System.Collections.Generic;
using Domain.Posts;

namespace Domain.Users
{
    public partial class User
    {
        public static CreationResult<User> Create(UserCreateCommand command)
        {
            if (command.Name.Length > 4) {
                var newGuid = Guid.NewGuid();
                var user = new User(newGuid, command);
                return CreationResult<User>.OkResult(new List<DomainEventBase> {new UserCreateEvent(user, newGuid)}, user);
            }

            return CreationResult<User>.ErrorResult(new List<string> {"Name too short"});
        }

        public ValidationResult UpdateAge(UserUpdateAgeCommand command)
        {
            Age = command.Age;
            return ValidationResult.OkResult(new List<DomainEventBase> { new UserUpdateAgeEvent(Age, Id) });
        }

        public ValidationResult UpdateName(UserUpdateNameCommand command)
        {
            var creationResult = Post.Create(new PostCreateCommand("luly"));
            Posts.Add(creationResult.CreatedEntity);
            return ValidationResult.OkResult(new List<DomainEventBase>());
        }

        public ValidationResult AddPost(UserAddPostCommand command)
        {
            if (Posts.Count < 20)
            {
                Posts.Add(command.Post);
                return ValidationResult.OkResult(new List<DomainEventBase>{new PostAddEvent(Id, command.Post.Id)});
            }
            return ValidationResult.ErrorResult(new List<string>{"Can not add more than 20 Posts"});
        }
    }

    public class PostAddEvent : DomainEventBase
    {
        public Guid PostId { get; private set; }

        public PostAddEvent(Guid Id, Guid PostId) : base(Id)
        {
            this.PostId = PostId;
        }
    }
}