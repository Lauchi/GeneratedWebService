using System;

namespace Domain.Posts
{
    public partial class Post
    {
        public static PostCreateEvent Create(string title)
        {
            var newGuid = Guid.NewGuid();
            return new PostCreateEvent(new Post(newGuid, title), newGuid);
        }
    }
}