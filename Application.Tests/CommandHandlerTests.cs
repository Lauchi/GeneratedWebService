using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Posts;
using Application.Users;
using Domain;
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
    }
}