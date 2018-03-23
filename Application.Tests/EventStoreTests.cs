using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Users.Hooks;
using Domain;
using Domain.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Application.Tests
{
    [TestClass]
    public class EventStoreTests
    {
        [TestMethod]
        public async Task GetByIdMethod()
        {
            var eventRepo = new Mock<IEventStoreRepository>();
            eventRepo.Setup(repo => repo.AddEvents(It.IsAny<List<DomainEventBase>>())).Returns(Task.FromResult(true));
            var eventStore = new EventStore(eventRepo.Object, new SendPasswordMailHook());

            var hookResult = await eventStore.AppendAll(new List<DomainEventBase>
            {
                new UserCreateEvent(User.Create(new UserCreateCommand("Peter", 12)).CreatedEntity, Guid.NewGuid())
            });

            Assert.IsTrue(hookResult.Ok);
        }
    }
}