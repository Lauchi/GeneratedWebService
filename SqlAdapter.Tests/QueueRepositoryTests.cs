using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application;
using Domain.Users;
using Microsoft.AspNetCore.Antiforgery.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlAdapter.Tests
{
    [TestClass]
    public class QueueRepositoryTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task AddEvents()
        {
            _hangfireContext.Database.EnsureCreated();

            var queueRepository = new QueueRepository(_hangfireContext);

            var eventAndJob = new EventAndJob(new UserUpdateNameEvent("Dude", Guid.NewGuid()), "Job");
            await queueRepository.AddEventForJob(eventAndJob);

            var events = await queueRepository.GetEvents("Job");

            Assert.AreEqual(1, events.Count);
            Assert.AreEqual(eventAndJob, events[0]);
        }

        [TestMethod]
        public async Task RemoveEvents()
        {
            _hangfireContext.Database.EnsureCreated();

            var queueRepository = new QueueRepository(_hangfireContext);

            var eventAndJob = new EventAndJob(new UserUpdateNameEvent("Dude", Guid.NewGuid()), "Job");
            await queueRepository.AddEventForJob(eventAndJob);

            await queueRepository.RemoveEventsFromQueue(new List<EventAndJob> { eventAndJob });
            var events = await queueRepository.GetEvents("Job");

            Assert.AreEqual(0, events.Count);
        }

        [TestMethod]
        public async Task GetEvents()
        {
            _hangfireContext.Database.EnsureCreated();

            var queueRepository = new QueueRepository(_hangfireContext);

            var eventAndJob = new EventAndJob(new UserUpdateNameEvent("Dude", Guid.NewGuid()), "Job");
            var eventAndJob2 = new EventAndJob(new UserUpdateNameEvent("Dude", Guid.NewGuid()), "JobSecond");
            await queueRepository.AddEventForJob(eventAndJob);
            await queueRepository.AddEventForJob(eventAndJob2);

            var events = await queueRepository.GetEvents("Job");

            Assert.AreEqual(1, events.Count);
            Assert.AreEqual(eventAndJob, events[0]);
        }
    }
}