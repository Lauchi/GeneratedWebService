using System.Threading.Tasks;

namespace AsyncHost
{
    public interface IRowVersionRepository
    {
        long GetVersion<T>();
        Task SaveVersion<T>();
    }

    class RowVersionRepository : IRowVersionRepository
    {
        public long GetVersion<T>()
        {
            return 20;
        }

        public Task SaveVersion<T>()
        {
            return Task.FromResult(true);
        }
    }
}