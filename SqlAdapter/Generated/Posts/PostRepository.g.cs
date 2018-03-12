//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SqlAdapter.Posts
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Posts;
    using Application.Posts;
    using Microsoft.EntityFrameworkCore;
    
    
    public class PostRepository : IPostRepository
    {
        
        public EventStoreContext EventStore { get; private set; }
        
        public PostRepository(EventStoreContext EventStore)
        {
            this.EventStore = EventStore;
        }
        
        public async Task CreatePost(Post post)
        {
            EventStore.Posts.Add(post);
            await EventStore.SaveChangesAsync();;
        }
        
        public async Task UpdatePost(Post post)
        {
            EventStore.Posts.Update(post);
            await EventStore.SaveChangesAsync();;
        }
        
        public async Task<Post> GetPost(Guid id)
        {
            return await EventStore.Posts.FindAsync(id);
        }
        
        public async Task<List<Post>> GetPosts()
        {
            return await EventStore.Posts.ToListAsync();
        }
    }
}
