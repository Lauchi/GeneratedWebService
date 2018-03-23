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
    }
}