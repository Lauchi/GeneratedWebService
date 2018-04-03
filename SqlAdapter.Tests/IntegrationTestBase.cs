using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlAdapter.Tests
{
    public class IntegrationTestBase
    {
        protected SqliteConnection _connection;
        protected HangfireContext _hangfireContext;

        [TestInitialize]
        public void Setup()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
            var options = new DbContextOptionsBuilder<HangfireContext>()
                .UseSqlite(_connection)
                .Options;
            _hangfireContext = new HangfireContext(options);

        }

        [TestCleanup]
        public void Clean()
        {
            _connection.Close();
        }
    }
}