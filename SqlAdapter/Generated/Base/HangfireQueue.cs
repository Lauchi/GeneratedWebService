using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace SqlAdapter
{
    public class HangfireQueue : IHangfireQueue
    {
        private readonly HangfireContext _context;

        private List<EventTuple> RegisteredJobs = new List<EventTuple>();

        public HangfireQueue(HangfireContext context)
        {
            _context = context;
            RegisteredJobs.Add(new EventTuple(typeof(UserUpdateAgeEvent).ToString(), "SendBirthdayMail"));
            RegisteredJobs.Add(new EventTuple(typeof(UserCreateEvent).ToString(), "SendPasswordMail"));
            RegisteredJobs.Add(new EventTuple(typeof(UserCreateEvent).ToString(), "SendWelcomeMail"));
        }

        public async Task AddEvents(List<DomainEventBase> domainEvents)
        {
            foreach (var domainEvent in domainEvents)
            {
                var jobsThatDoEvents = RegisteredJobs.Where(tuple => domainEvent.GetType().ToString() == tuple.DomainType);
                foreach (var job in jobsThatDoEvents)
                {
                    var combination = new EventAndJob(domainEvent, job.JobName);
                    _context.EventAndJobQueue.Add(combination);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<EventAndJob>> GetEvents(string jobName)
        {
            var eventList = await _context.EventAndJobQueue.Where(eve => eve.JobName == jobName).ToListAsync();
            return eventList;
        }

        public async Task RemoveEventsFromQueue(List<EventAndJob> handledEvents)
        {
            _context.EventAndJobQueue.RemoveRange(handledEvents);
            await _context.SaveChangesAsync();
        }
    }
}