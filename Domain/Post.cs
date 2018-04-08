using System;
using System.Collections.Generic;

namespace Domain.Posts
{
    public partial class Post
    {
        public static CreationResult<Post> Create(PostCreateCommand command)
        {
            var newGuid = Guid.NewGuid();
            var post = new Post(newGuid, command);
            var createEvent = new PostCreateEvent(post, newGuid);
            var domainEventBases = new List<DomainEventBase>();
            domainEventBases.Add(createEvent);
            return CreationResult<Post>.OkResult(domainEventBases, post);
        }

        public ValidationResult UpdateTitle(PostUpdateTitleCommand command)
        {
            Title = command.Title;
            return ValidationResult.OkResult(new List<DomainEventBase>{new PostUpdateTitleEvent(Title, Id)});
        }
    }
}