using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace SqlAdapter
{
    public class HangfireQueue : IHangfireQueue
    {
        private readonly HangfireContext _context;

        private readonly List<EventTuple> _registeredJobs = new List<EventTuple>();

        public HangfireQueue(HangfireContext context, EventJobRegistration registration)
        {
            _context = context;
            foreach (var eventJob in registration.EventJobs)
            {
                _registeredJobs.Add(eventJob);
            }
        }

        public async Task AddEvents(List<DomainEventBase> domainEvents)
        {
            foreach (var domainEvent in domainEvents)
            {
                var jobsTriggereByEvent = _registeredJobs.Where(tuple => domainEvent.GetType().ToString() == tuple.DomainType);
                foreach (var job in jobsTriggereByEvent)
                {
                    var combination = new EventAndJob(domainEvent, job.JobName);
                    _context.EventAndJobQueue.Add(combination);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<EventAndJob>> GetEvents(string jobName)
        {
            var eventList = await _context.EventAndJobQueue.Include(queue => queue.DomainEvent).Where(eve => eve.JobName == jobName).ToListAsync();
            return eventList;
        }

        public async Task RemoveEventsFromQueue(List<EventAndJob> handledEvents)
        {
            _context.EventAndJobQueue.RemoveRange(handledEvents);
            await _context.SaveChangesAsync();
        }
    }
}