using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Posts;
using Application.Users;
using Domain;
using Domain.Posts;
using Domain.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Application.Tests
{
    [TestClass]
    public class CommandHandlerTests
    {
        [TestMethod]
        public async Task GetByIdMethod()
        {
            var eventStore = new Mock<IEventStore>();
            eventStore.Setup(store => store.AppendAll(It.IsAny<List<DomainEventBase>>()))
                .ReturnsAsync(HookResult.OkResult());
            var postRepo = new Mock<IPostRepository>();
            var userRepo = new Mock<IUserRepository>();

            var creationResult = User.Create(new UserCreateCommand("Peter", 12)).CreatedEntity;
            var searchGuid = Guid.NewGuid();
            userRepo.Setup(repo => repo.GetUser(searchGuid)).ReturnsAsync(creationResult);

            var userCommandHandler = new UserCommandHandler(eventStore.Object, userRepo.Object, postRepo.Object);

            var user = await userCommandHandler.GetUser(searchGuid);
            Assert.AreEqual("Peter", ((User)((JsonResult)user).Value).Name);
            Assert.AreEqual(12, ((User)((JsonResult)user).Value).Age);
        }


        [TestMethod]
        public async Task GetByIdMethod_NotFound()
        {
            var eventStore = new Mock<IEventStore>();
            eventStore.Setup(store => store.AppendAll(It.IsAny<List<DomainEventBase>>()))
                .ReturnsAsync(HookResult.OkResult());
            var postRepo = new Mock<IPostRepository>();
            var userRepo = new Mock<IUserRepository>();

            var searchGuid = Guid.NewGuid();
            userRepo.Setup(repo => repo.GetUser(searchGuid)).ReturnsAsync((User) null);

            var userCommandHandler = new UserCommandHandler(eventStore.Object, userRepo.Object, postRepo.Object);

            var result = await userCommandHandler.GetUser(searchGuid);
            Assert.AreEqual(404, ((NotFoundObjectResult)result).StatusCode);
        }


        [TestMethod]
        public async Task GetByAllMethod()
        {
            var eventStore = new Mock<IEventStore>();
            eventStore.Setup(store => store.AppendAll(It.IsAny<List<DomainEventBase>>()))
                .ReturnsAsync(HookResult.OkResult());
            var postRepo = new Mock<IPostRepository>();
            var userRepo = new Mock<IUserRepository>();

            List<User> userList = new List<User>
            {
                User.Create(new UserCreateCommand("Peter", 18)).CreatedEntity,
                User.Create(new UserCreateCommand("Maier", 21)).CreatedEntity
            };
            userRepo.Setup(repo => repo.GetUsers()).ReturnsAsync(userList);

            var userCommandHandler = new UserCommandHandler(eventStore.Object, userRepo.Object, postRepo.Object);

            var result = await userCommandHandler.GetUsers();
            Assert.AreEqual(userList, (List<User>)((OkObjectResult)result).Value);
        }

        [TestMethod]
        public async Task GetByAllMethod_EmptyList()
        {
            var eventStore = new Mock<IEventStore>();
            eventStore.Setup(store => store.AppendAll(It.IsAny<List<DomainEventBase>>()))
                .ReturnsAsync(HookResult.OkResult());
            var postRepo = new Mock<IPostRepository>();
            var userRepo = new Mock<IUserRepository>();

            List<User> userList = new List<User>();
            userRepo.Setup(repo => repo.GetUsers()).ReturnsAsync(userList);

            var userCommandHandler = new UserCommandHandler(eventStore.Object, userRepo.Object, postRepo.Object);

            var result = await userCommandHandler.GetUsers();
            Assert.AreEqual(userList, (List<User>)((OkObjectResult)result).Value);
        }

        [TestMethod]
        public async Task CreateUser_HappyPath()
        {
            var eventStore = new Mock<IEventStore>();
            eventStore.Setup(store => store.AppendAll(It.IsAny<List<DomainEventBase>>()))
                .ReturnsAsync(HookResult.OkResult());
            var postRepo = new Mock<IPostRepository>();

            var userRepo = new Mock<IUserRepository>();

            var userCreateCommand = new UserCreateCommand("Peter", 18);
            var createdEntity = User.Create(userCreateCommand).CreatedEntity;
            userRepo.Setup(repo => repo.CreateUser(createdEntity)).Returns(Task.FromResult(true));

            var userCommandHandler = new UserCommandHandler(eventStore.Object, userRepo.Object, postRepo.Object);

            var result = await userCommandHandler.CreateUser(userCreateCommand);
            Assert.AreEqual(201, ((CreatedResult)result).StatusCode);
            Assert.AreEqual(18, ((User)((CreatedResult)result).Value).Age);
            Assert.AreEqual("Peter", ((User)((CreatedResult)result).Value).Name);
        }

        [TestMethod]
        public async Task CreateUser_HookFails()
        {
            var eventStore = new Mock<IEventStore>();
            var errors = new List<string> { "Some Error" };
            eventStore.Setup(store => store.AppendAll(It.IsAny<List<DomainEventBase>>()))
                .ReturnsAsync(HookResult.ErrorResult(errors));
            var postRepo = new Mock<IPostRepository>();
            var userRepo = new Mock<IUserRepository>();

            var userCommandHandler = new UserCommandHandler(eventStore.Object, userRepo.Object, postRepo.Object);

            var result = await userCommandHandler.CreateUser(new UserCreateCommand("Peter", 18));
            Assert.AreEqual(400, ((BadRequestObjectResult)result).StatusCode);
            Assert.AreEqual(errors, (List<string>)((BadRequestObjectResult)result).Value);
        }

        [TestMethod]
        public async Task CreateUser_CreateFails()
        {
            var eventStore = new Mock<IEventStore>();
            var postRepo = new Mock<IPostRepository>();
            var userRepo = new Mock<IUserRepository>();

            var userCommandHandler = new UserCommandHandler(eventStore.Object, userRepo.Object, postRepo.Object);

            var result = await userCommandHandler.CreateUser(new UserCreateCommand("Pe", 18));
            Assert.AreEqual(400, ((BadRequestObjectResult)result).StatusCode);
            Assert.AreEqual(1, ((List<string>)((BadRequestObjectResult)result).Value).Count);
            Assert.AreEqual("Name too short", ((List<string>)((BadRequestObjectResult)result).Value)[0]);
        }

        [TestMethod]
        public async Task UpdateNameUser_HappyPath()
        {
            var eventStore = new Mock<IEventStore>();
            eventStore.Setup(store => store.AppendAll(It.IsAny<List<DomainEventBase>>()))
                .ReturnsAsync(HookResult.OkResult());
            var postRepo = new Mock<IPostRepository>();
            var userRepo = new Mock<IUserRepository>();
            var updateId = Guid.NewGuid();
            var user = User.Create(new UserCreateCommand("Peter", 13)).CreatedEntity;
            userRepo.Setup(repo => repo.GetUser(updateId)).ReturnsAsync(user);

            var userCommandHandler = new UserCommandHandler(eventStore.Object, userRepo.Object, postRepo.Object);

            var result = await userCommandHandler.UpdateNameUser(updateId, new UserUpdateNameCommand("NeuerPeter"));
            Assert.AreEqual(200, ((OkResult)result).StatusCode);
        }

        [TestMethod]
        public async Task UpdateNameUser_UserNotFound()
        {
            var eventStore = new Mock<IEventStore>();
            var postRepo = new Mock<IPostRepository>();
            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(repo => repo.GetUser(Guid.NewGuid())).ReturnsAsync((User) null);

            var userCommandHandler = new UserCommandHandler(eventStore.Object, userRepo.Object, postRepo.Object);

            var result = await userCommandHandler.UpdateNameUser(Guid.NewGuid(), new UserUpdateNameCommand("NeuerPeter"));
            Assert.AreEqual(404, ((NotFoundObjectResult)result).StatusCode);
        }

        [TestMethod]
        public async Task UpdateNameUser_UserUpdateNameError()
        {
            var eventStore = new Mock<IEventStore>();
            var postRepo = new Mock<IPostRepository>();
            var userRepo = new Mock<IUserRepository>();
            var updateId = Guid.NewGuid();
            var user = User.Create(new UserCreateCommand("Peter", 13)).CreatedEntity;
            userRepo.Setup(repo => repo.GetUser(updateId)).ReturnsAsync(user);

            var userCommandHandler = new UserCommandHandler(eventStore.Object, userRepo.Object, postRepo.Object);

            var result = await userCommandHandler.UpdateNameUser(updateId, new UserUpdateNameCommand("Np"));
            Assert.AreEqual(400, ((BadRequestObjectResult)result).StatusCode);
            Assert.AreEqual("Name too short to update", ((List<string>)((BadRequestObjectResult)result).Value)[0]);
        }

        [TestMethod]
        public async Task UpdateNameUser_HookFailing()
        {
            var eventStore = new Mock<IEventStore>();
            var errors = new List<string>{"error" };
            eventStore.Setup(store => store.AppendAll(It.IsAny<List<DomainEventBase>>()))
                .ReturnsAsync(HookResult.ErrorResult(errors));

            var postRepo = new Mock<IPostRepository>();
            var userRepo = new Mock<IUserRepository>();
            var updateId = Guid.NewGuid();
            var user = User.Create(new UserCreateCommand("Peter", 13)).CreatedEntity;
            userRepo.Setup(repo => repo.GetUser(updateId)).ReturnsAsync(user);

            var userCommandHandler = new UserCommandHandler(eventStore.Object, userRepo.Object, postRepo.Object);

            var result = await userCommandHandler.UpdateNameUser(updateId, new UserUpdateNameCommand("NeuerPeter"));
            Assert.AreEqual(400, ((BadRequestObjectResult)result).StatusCode);
            Assert.AreEqual(errors, (List<string>)((BadRequestObjectResult)result).Value);
        }

        [TestMethod]
        public async Task LoadMethod_HappyPath()
        {
            var eventStore = new Mock<IEventStore>();
            eventStore.Setup(store => store.AppendAll(It.IsAny<List<DomainEventBase>>()))
                .ReturnsAsync(HookResult.OkResult);

            var postRepo = new Mock<IPostRepository>();
            var userRepo = new Mock<IUserRepository>();
            var updateId = Guid.NewGuid();
            var updateId2 = Guid.NewGuid();
            var post = Post.Create(new PostCreateCommand("Peters Post")).CreatedEntity;
            var post2 = Post.Create(new PostCreateCommand("Peters Post")).CreatedEntity;
            var user = User.Create(new UserCreateCommand("Peter", 13)).CreatedEntity;

            postRepo.Setup(repo => repo.GetPost(updateId)).ReturnsAsync(post);
            postRepo.Setup(repo => repo.GetPost(updateId2)).ReturnsAsync(post2);
            userRepo.Setup(repo => repo.GetUser(updateId)).ReturnsAsync(user);

            var userCommandHandler = new UserCommandHandler(eventStore.Object, userRepo.Object, postRepo.Object);

            var result = await userCommandHandler.AddPostUser(updateId, new UserAddPostApiCommand(updateId, updateId2));
            Assert.AreEqual(200, ((OkResult)result).StatusCode);
        }

        [TestMethod]
        public async Task LoadMethod_DomainError()
        {
            var eventStore = new Mock<IEventStore>();

            var postRepo = new Mock<IPostRepository>();
            var userRepo = new Mock<IUserRepository>();
            var updateId = Guid.NewGuid();
            var post = Post.Create(new PostCreateCommand("Peters Post")).CreatedEntity;
            var user = User.Create(new UserCreateCommand("Peter", 13)).CreatedEntity;

            postRepo.Setup(repo => repo.GetPost(updateId)).ReturnsAsync(post);
            userRepo.Setup(repo => repo.GetUser(updateId)).ReturnsAsync(user);

            var userCommandHandler = new UserCommandHandler(eventStore.Object, userRepo.Object, postRepo.Object);

            var result = await userCommandHandler.AddPostUser(updateId, new UserAddPostApiCommand(updateId, updateId));
            Assert.AreEqual(400, ((BadRequestObjectResult)result).StatusCode);
            Assert.AreEqual(1, ((List<string>)((BadRequestObjectResult)result).Value).Count);
            Assert.AreEqual("Can not delete post that should be added", ((List<string>)((BadRequestObjectResult)result).Value)[0]);
        }

        [TestMethod]
        public async Task LoadMethod_HookResultError()
        {
            var eventStore = new Mock<IEventStore>();
            var errors = new List<string>{"error"};
            eventStore.Setup(store => store.AppendAll(It.IsAny<List<DomainEventBase>>()))
                .ReturnsAsync(HookResult.ErrorResult(errors));

            var postRepo = new Mock<IPostRepository>();
            var userRepo = new Mock<IUserRepository>();
            var updateId = Guid.NewGuid();
            var updateId2 = Guid.NewGuid();
            var post = Post.Create(new PostCreateCommand("Peters Post")).CreatedEntity;
            var post2 = Post.Create(new PostCreateCommand("Peters Post")).CreatedEntity;
            var user = User.Create(new UserCreateCommand("Peter", 13)).CreatedEntity;

            postRepo.Setup(repo => repo.GetPost(updateId)).ReturnsAsync(post);
            postRepo.Setup(repo => repo.GetPost(updateId2)).ReturnsAsync(post2);
            userRepo.Setup(repo => repo.GetUser(updateId)).ReturnsAsync(user);

            var userCommandHandler = new UserCommandHandler(eventStore.Object, userRepo.Object, postRepo.Object);

            var result = await userCommandHandler.AddPostUser(updateId, new UserAddPostApiCommand(updateId, updateId2));
            Assert.AreEqual(400, ((BadRequestObjectResult)result).StatusCode);
            Assert.AreEqual(errors, (List<string>)((BadRequestObjectResult)result).Value);
        }

        [TestMethod]
        public async Task LoadMethod_RootNotFound()
        {
            var eventStore = new Mock<IEventStore>();

            var postRepo = new Mock<IPostRepository>();
            var userRepo = new Mock<IUserRepository>();

            userRepo.Setup(repo => repo.GetUser(It.IsAny<Guid>())).ReturnsAsync((User) null);

            var userCommandHandler = new UserCommandHandler(eventStore.Object, userRepo.Object, postRepo.Object);

            var userGuid = Guid.NewGuid();
            var result = await userCommandHandler.AddPostUser(userGuid, new UserAddPostApiCommand(Guid.NewGuid(), Guid.NewGuid()));
            Assert.AreEqual(404, ((NotFoundObjectResult)result).StatusCode);
            var errors = $"Could not find Root User with ID: {userGuid}";
            Assert.AreEqual(1, ((List<string>)((NotFoundObjectResult)result).Value).Count);
            Assert.AreEqual(errors, ((List<string>)((NotFoundObjectResult)result).Value)[0]);
        }
    }
}