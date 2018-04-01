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
            if (command.Name.Length > 4) {
                Posts.Add(creationResult.CreatedEntity);
                return ValidationResult.OkResult(new List<DomainEventBase> { new UserUpdateNameEvent(command.Name, Id) });
            }
            return ValidationResult.ErrorResult(new List<string>{"Name too short to update"});

        }

        public ValidationResult AddPost(UserAddPostCommand command)
        {
            if (command.NewPost != null)
            {
                Posts.Add(command.NewPost);
                return ValidationResult.OkResult(new List<DomainEventBase>{new UserAddPostEvent(command.NewPost.Id, Id) });
            }
            return ValidationResult.ErrorResult(new List<string>{"Can not delete post that should be added"});
        }
    }
}