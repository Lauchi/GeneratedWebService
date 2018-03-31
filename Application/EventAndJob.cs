using System;
using Domain;

namespace Application
{
    public class EventAndJob
    {
        public Guid Id { get; private set; }
        public EventAndJob(DomainEventBase domainEventBase, string jobName)
        {
            DomainEvent = domainEventBase;
            JobName = jobName;
            Id = Guid.NewGuid();
        }

        public EventAndJob()
        {
        }

        public DomainEventBase DomainEvent { get; private set; }
        public string JobName { get; private set; }
    }
}