using System.Collections.Generic;
using Domain.Users;

namespace SqlAdapter
{
    public class EventJobRegistration
    {
        public EventJobRegistration()
        {
            EventJobs.Add(new EventTuple(typeof(UserUpdateAgeEvent).ToString(), "SendBirthdayMail"));
            EventJobs.Add(new EventTuple(typeof(UserCreateEvent).ToString(), "SendPasswordMail"));
            EventJobs.Add(new EventTuple(typeof(UserCreateEvent).ToString(), "SendWelcomeMail"));
        }

        public List<EventTuple> EventJobs { get; set; } = new List<EventTuple>();
    }
}