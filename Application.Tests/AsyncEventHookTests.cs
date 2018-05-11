using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Users;
using Application.Users.AsyncHooks;
using Domain.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Application.Tests
{
    [TestClass]
    public class AsyncEventHookTests
    {
        [TestMethod]
        public async Task CreatMethod_HappyPath()
        {
            var userRepo = new Mock<IUserRepository>();
            var hangfireQueue = new Mock<IHangfireQueue>();
            var creationResult = User.Create(new UserCreateCommand("Peter", 12));
            var searchGuid = Guid.NewGuid();
            userRepo.Setup(repo => repo.GetUser(searchGuid)).ReturnsAsync(creationResult.CreatedEntity);
            EventAndJob job = new EventAndJob(creationResult.DomainEvents[0], "SendPasswordMail");
            hangfireQueue.Setup(hangfire => hangfire.GetEvents("SendPasswordMail"))
                .ReturnsAsync(new List<EventAndJob> {job});

            var onUserCreateSendPasswordMailEventHandler = new OnUserCreateSendPasswordMailEventHandler(new OnUserCreateSendPasswordMailAsyncHook(), hangfireQueue.Object, userRepo.Object);
            await onUserCreateSendPasswordMailEventHandler.Run();

            hangfireQueue.Verify(hangfire => hangfire.RemoveEventsFromQueue(It.Is<List<EventAndJob>>(list => list.Contains(job) && list.Count == 1)), Times.Once);
        }

        [TestMethod]
        public async Task CreatMethod_Error()
        {
            var userRepo = new Mock<IUserRepository>();
            var hangfireQueue = new Mock<IHangfireQueue>();
            var creationResult = User.Create(new UserCreateCommand("Peter", 12));
            var searchGuid = Guid.NewGuid();
            userRepo.Setup(repo => repo.GetUser(searchGuid)).ReturnsAsync(creationResult.CreatedEntity);
            EventAndJob job = new EventAndJob(creationResult.DomainEvents[0], "SendWelcomeMail");
            hangfireQueue.Setup(hangfire => hangfire.GetEvents("SendWelcomeMail"))
                .ReturnsAsync(new List<EventAndJob> {job});

            var onUserCreateSendPasswordMailEventHandler = new OnUserCreateSendWelcomeMailEventHandler(new OnUserCreateSendWelcomeMailAsyncHook(), hangfireQueue.Object, userRepo.Object);
            await onUserCreateSendPasswordMailEventHandler.Run();

            hangfireQueue.Verify(hangfire => hangfire.RemoveEventsFromQueue(It.Is<List<EventAndJob>>(list => list.Contains(job) && list.Count == 1)), Times.Never);
        }

        [TestMethod]
        public async Task UpdateMethod_HappyPath()
        {
            var hangfireQueue = new Mock<IHangfireQueue>();
            EventAndJob job = new EventAndJob(new UserUpdateAgeEvent(15, Guid.NewGuid()), "SendBirthdayMail");
            hangfireQueue.Setup(hangfire => hangfire.GetEvents("SendBirthdayMail"))
                .ReturnsAsync(new List<EventAndJob> { job });

            var onUserCreateSendPasswordMailEventHandler = new OnUserUpdateAgeSendBirthdayMailEventHandler(new OnUserUpdateAgeSendBirthdayMailAsyncHook(), hangfireQueue.Object);
            await onUserCreateSendPasswordMailEventHandler.Run();

            hangfireQueue.Verify(hangfire => hangfire.RemoveEventsFromQueue(It.Is<List<EventAndJob>>(list => list.Contains(job) && list.Count == 1)), Times.Once);
        }
    }
}