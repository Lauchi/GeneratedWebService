namespace SqlAdapter
{
    public class EntityRowVersion
    {
        public string EventType  { get; private set; }
        public long LastRowVersion  { get; set; }
    }
}