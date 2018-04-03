using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlAdapter.Tests
{
    public class IntegrationTestBase
    {
        protected SqliteConnection _connectionHangfire;
        protected SqliteConnection _connectionEventStore;
        protected HangfireContext _hangfireContext;
        protected EventStoreContext _eventStoreContext;

        [TestInitialize]
        public void Setup()
        {
            _connectionHangfire = new SqliteConnection("DataSource=:memory:");
            _connectionHangfire.Open();
            var options = new DbContextOptionsBuilder<HangfireContext>()
                .UseSqlite(_connectionHangfire)
                .Options;
            _hangfireContext = new HangfireContext(options);

            _connectionEventStore = new SqliteConnection("DataSource=:memory:");
            _connectionEventStore.Open();
            var optionsEventStore = new DbContextOptionsBuilder<EventStoreContext>()
                .UseSqlite(_connectionEventStore)
                .Options;
            _eventStoreContext = new EventStoreContext(optionsEventStore);

            _hangfireContext.Database.EnsureCreated();
            _eventStoreContext.Database.EnsureCreated();
        }

        [TestCleanup]
        public void Clean()
        {
            _connectionHangfire.Close();
        }
    }
}