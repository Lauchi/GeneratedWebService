using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Posts;
using Application.Users;
using Application.Users.AsyncHooks;
using Domain.Posts;
using Domain.Queries;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace SqlAdapter
{
    class QueryRepo : IQueryRepo
    {
        private readonly EventStoreContext _context;
        private readonly IUserRepository _userRepository;

        public QueryRepo(EventStoreContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        public async Task<List<UserWithPostNumberQuery>> Load(PostUpdateTitleEvent domainEvent)
        {
            var parent = await _userRepository.GetMyPostsParent(domainEvent.EntityId);
            var parent2 = await _userRepository.GetPinnedPostParent(domainEvent.EntityId);

            var queryFromDb = _context.UserWithPostNumberQuerys.Where(query => query.RootEntityId == parent.Id);
            var queryFromDb2 = _context.UserWithPostNumberQuerys.Where(query => query.RootEntityId == parent2.Id);

            var userWithPostNumberQueries = new List<UserWithPostNumberQuery>();

            userWithPostNumberQueries.AddRange(queryFromDb);
            userWithPostNumberQueries.AddRange(queryFromDb2);
            return userWithPostNumberQueries;
        }

        public Task<List<UserWithPostNumberQuery>> Load(UserUpdateNameEvent domainEvent)
        {
            return Task.FromResult(_context.UserWithPostNumberQuerys.Where(query => query.RootEntityId == domainEvent.EntityId).ToList());
        }

        public async Task Update(UserWithPostNumberQuery withPostNumberQuery)
        {
            _context.UserWithPostNumberQuerys.Update(withPostNumberQuery);
            await _context.SaveChangesAsync();
        }

        public async Task Create(UserWithPostNumberQuery userWithPostNumberQuery)
        {
            _context.UserWithPostNumberQuerys.Add(userWithPostNumberQuery);
            await _context.SaveChangesAsync();
        }
    }
}