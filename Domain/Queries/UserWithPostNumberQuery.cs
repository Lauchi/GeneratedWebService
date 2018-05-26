using System;
using Domain.Posts;
using Domain.Users;

namespace Domain.Queries
{
    public class UserWithPostNumberQuery
    {
        public static UserWithPostNumberQuery Create(UserCreateEvent domainEvent)
        {
            return new UserWithPostNumberQuery
            {
                Id = Guid.NewGuid(),
                RootEntityId = domainEvent.EntityId,
                Name = domainEvent.User.Name,
                PostNumber = 0,
                LatestTitleChange = "No changes yet"
            };
        }

        public Guid Id { get; set; }

        public Guid RootEntityId { get; set; }

        public void Apply(UserAddPostEvent domainEvent)
        {
            PostNumber += 1;
        }

        public int PostNumber { get; set; }

        public void Apply(UserUpdateNameEvent domainEvent)
        {
            Name = domainEvent.Name;
        }

        public string Name { get; set; }

        public void Apply(PostUpdateTitleEvent domainEvent)
        {
            LatestTitleChange = domainEvent.Title;
        }

        public string LatestTitleChange { get; set; }
    }
}