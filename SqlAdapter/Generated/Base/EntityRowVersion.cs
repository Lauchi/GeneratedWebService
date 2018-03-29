using System.ComponentModel.DataAnnotations;

namespace SqlAdapter.Generated.Base
{
    public class EntityRowVersion
    {
        public EntityRowVersion()
        {
        }

        public EntityRowVersion(string eventType)
        {
            EventType = eventType;
        }

        [Key]
        public string EventType  { get; private set; }

        public long LastRowVersion { get; set; }
    }
}