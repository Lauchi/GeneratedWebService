using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Posts;
using Domain.Users;

namespace Application.Posts
{
    public interface IPostRepository
    {
        Task<User> GetPost(Guid id);
        Task UpdatePost(Post parsedUser);
        Task CreatePost(Post userEventUser);
        Task<IList<User>> GetPosts();
    }
}