using System.Threading.Tasks;
using Domain.Posts;
using Domain.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlAdapter.Posts;
using SqlAdapter.Users;

namespace SqlAdapter.Tests
{
    [TestClass]
    public class EntityRepositoryTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task AddPost()
        {
            var postRepository = new PostRepository(_eventStoreContext);

            var expectedTitle = "MyPost";
            var creationResult = Post.Create(new PostCreateCommand(expectedTitle));
            await postRepository.CreatePost(creationResult.CreatedEntity);

            var posts = await postRepository.GetPosts();
            Assert.AreEqual(1, posts.Count);
            Assert.AreEqual(expectedTitle, posts[0].Title);
        }

        [TestMethod]
        public async Task GetPost()
        {
            var postRepository = new PostRepository(_eventStoreContext);

            var expectedTitle = "MyPost";
            var creationResult = Post.Create(new PostCreateCommand(expectedTitle));
            await postRepository.CreatePost(creationResult.CreatedEntity);

            var post = await postRepository.GetPost(creationResult.CreatedEntity.Id);
            Assert.AreEqual(expectedTitle, post.Title);
            Assert.AreEqual(creationResult.CreatedEntity.Id, post.Id);
        }

        [TestMethod]
        public async Task UpdatePost()
        {
            var userRepository = new UserRepository(_eventStoreContext);

            var creationResult = User.Create(new UserCreateCommand("Peter", 13));
            var user = creationResult.CreatedEntity;
            await userRepository.CreateUser(user);

            user.UpdateAge(new UserUpdateAgeCommand(20));

            await userRepository.UpdateUser(user);
            await userRepository.GetUser(user.Id);

            Assert.AreEqual(20, user.Age);
        }

        [TestMethod]
        public async Task GetPostParent()
        {
            var userRepository = new UserRepository(_eventStoreContext);
            var postRepository = new PostRepository(_eventStoreContext);

            var expectedName = "Peter";
            var post = Post.Create(new PostCreateCommand("testTitle")).CreatedEntity;
            var post2 = Post.Create(new PostCreateCommand("testTitle2")).CreatedEntity;
            var user = User.Create(new UserCreateCommand(expectedName, 12)).CreatedEntity;
            var user2 = User.Create(new UserCreateCommand("otherUser", 14)).CreatedEntity;
            await postRepository.CreatePost(post);
            user.MyPosts.Add(post);
            user.MyPosts.Add(post2);
            await userRepository.CreateUser(user);
            await userRepository.CreateUser(user2);

            var userFromDb = await userRepository.GetMyPostsParent(post.Id);
            Assert.AreEqual(expectedName, userFromDb.Name);
        }

        [TestMethod]
        public async Task GetPostParent_NoResult()
        {
            var userRepository = new UserRepository(_eventStoreContext);
            var postRepository = new PostRepository(_eventStoreContext);

            var expectedName = "Peter";
            var post = Post.Create(new PostCreateCommand("testTitle")).CreatedEntity;
            var user = User.Create(new UserCreateCommand(expectedName, 12)).CreatedEntity;
            await postRepository.CreatePost(post);
            await userRepository.CreateUser(user);

            var userFromDb = await userRepository.GetMyPostsParent(post.Id);
            Assert.IsNull(userFromDb);
        }
    }
}