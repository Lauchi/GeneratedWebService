using System.Linq;
using System.Threading.Tasks;
using SqlAdapter;

namespace AsyncHost
{
    public interface IRowVersionRepository
    {
        long GetVersion<T>();
        Task SaveVersion<T>(long lastRowVersion);
    }

    class RowVersionRepository : IRowVersionRepository
    {
        private readonly EventStoreContext _context;

        public RowVersionRepository(EventStoreContext context)
        {
            _context = context;
        }
        public long GetVersion<T>()
        {
            var entityRowVersion = _context.RowVersions.Single(rowVersion => rowVersion.EventType == typeof(T).ToString());
            return entityRowVersion.LastRowVersion;
        }

        public void SaveVersion<T>(long lastRowVersion)
        {
            var entityRowVersion = _context.RowVersions.Single(rowVersion => rowVersion.EventType == typeof(T).ToString());
            entityRowVersion.LastRowVersion = lastRowVersion;
            _context.RowVersions.Update(entityRowVersion);
        }
    }
}