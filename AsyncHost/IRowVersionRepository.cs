using System.Linq;
using System.Threading.Tasks;
using SqlAdapter;
using SqlAdapter.Generated.Base;

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
            var entityRowVersion = _context.RowVersions.SingleOrDefault(rowVersion => rowVersion.EventType == typeof(T).ToString());
            return entityRowVersion?.LastRowVersion ?? 0;
        }

        public async Task SaveVersion<T>(long lastRowVersion)
        {
            var entityRowVersion =
                _context.RowVersions.SingleOrDefault(rowVersion => rowVersion.EventType == typeof(T).ToString());
            if (entityRowVersion == null)
            {
                var newRowVersion = new EntityRowVersion(typeof(T).ToString());
                _context.RowVersions.Add(newRowVersion);

            } else
            {
                entityRowVersion.LastRowVersion = lastRowVersion;
                _context.RowVersions.Update(entityRowVersion);
            }
            await _context.SaveChangesAsync();
        }
    }
}