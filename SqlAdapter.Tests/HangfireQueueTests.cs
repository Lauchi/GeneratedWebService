using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application;
using Domain;
using Domain.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace SqlAdapter.Tests
{
    [TestClass]
    public class HangfireQueueTests
    {
        [TestMethod]
        public async Task AddEvents_OneEventIsNotRegisteredForJob()
        {
            var repository = new Mock<IQueueRepository>();

            var hangfireQueue = new HangfireQueue(new EventJobRegistration(), repository.Object);
            var userUpdateAgeEvent = new UserUpdateAgeEvent(14, Guid.NewGuid());
            var userUpdateNameEvent = new UserUpdateNameEvent("Peter", Guid.NewGuid());
            var events = new List<DomainEventBase> {userUpdateAgeEvent, userUpdateNameEvent, userUpdateAgeEvent};

            await hangfireQueue.AddEvents(events);

            repository.Verify(repo => repo.AddEventForJob(It.Is<EventAndJob>(job => job.DomainEvent == userUpdateAgeEvent)), Times.Exactly(2));
            repository.Verify(repo => repo.AddEventForJob(It.Is<EventAndJob>(job => job.DomainEvent == userUpdateNameEvent)), Times.Never);
        }

        [TestMethod]
        public async Task AddEvents_OneEvent()
        {
            var repository = new Mock<IQueueRepository>();

            var hangfireQueue = new HangfireQueue(new EventJobRegistration(), repository.Object);
            var userUpdateAgeEvent = new UserUpdateAgeEvent(14, Guid.NewGuid());
            var events = new List<DomainEventBase> {userUpdateAgeEvent};

            await hangfireQueue.AddEvents(events);

            repository.Verify(repo => repo.AddEventForJob(It.Is<EventAndJob>(job => job.DomainEvent == userUpdateAgeEvent)), Times.Once);
        }

        [TestMethod]
        public async Task AddEvents_NoEventIsRegeistered()
        {
            var repository = new Mock<IQueueRepository>();

            var hangfireQueue = new HangfireQueue(new EventJobRegistration(), repository.Object);
            var userUpdateNameEvent = new UserUpdateNameEvent("Peter", Guid.NewGuid());
            var events = new List<DomainEventBase> {userUpdateNameEvent};

            await hangfireQueue.AddEvents(events);

            repository.Verify(repo => repo.AddEventForJob(It.IsAny<EventAndJob>()), Times.Never);
        }
    }
}