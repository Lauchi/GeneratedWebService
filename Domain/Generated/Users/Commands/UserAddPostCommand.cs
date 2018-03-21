using Domain.Posts;

namespace Domain.Users
{
    public class UserAddPostCommand
    {
        public Post Post { get; private set; }

        public UserAddPostCommand(Post Post)
        {
            this.Post = Post;
        }

    }
}