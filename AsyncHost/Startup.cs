using Application.Users;
using Hangfire;
using Hangfire.SQLite;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlAdapter;
using SqlAdapter.Users;

namespace AsyncHost
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var options = new SQLiteStorageOptions();
            services.AddHangfire(configuration =>
                GlobalConfiguration.Configuration.UseSQLiteStorage("Data Source=Hangfire.db;", options))
                .AddTransient<OnUserCreateEventHandler>()
                .AddTransient<EventStoreContext>()
                .AddTransient<OnUserCreateEventHandler>()
                .AddTransient<IRowVersionRepository, RowVersionRepository>()
                .AddTransient<IUserRepository, UserRepository>()
                .AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            var option = new BackgroundJobServerOptions {WorkerCount = 1};
            app.UseHangfireServer(option);
            app.UseHangfireDashboard();

            app.UseMvc();
        }
    }
}