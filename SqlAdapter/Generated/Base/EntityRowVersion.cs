using System.ComponentModel.DataAnnotations;

namespace SqlAdapter.Generated.Base
{
    public class EntityRowVersion
    {
        [Key]
        public string EventType  { get; private set; }
        public long LastRowVersion  { get; set; }
    }
}