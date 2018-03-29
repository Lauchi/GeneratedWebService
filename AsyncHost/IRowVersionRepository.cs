namespace AsyncHost
{
    internal interface IRowVersionRepository
    {
        long GetUserCreateVersion();
    }

    class RowVersionRepository : IRowVersionRepository
    {
        public long GetUserCreateVersion()
        {
            return 20;
        }
    }
}