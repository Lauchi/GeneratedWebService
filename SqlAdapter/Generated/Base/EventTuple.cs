namespace SqlAdapter
{
    public class EventTuple
    {
        public EventTuple(string domainType, string jobName)
        {
            JobName = jobName;
            DomainType = domainType;
        }
        public string JobName { get; set; }
        public string DomainType { get; set; }
    }
}