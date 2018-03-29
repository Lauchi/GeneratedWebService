using System;
using System.ComponentModel;
using Application;
using Application.Users;
using Hangfire;
using Hangfire.SQLite;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
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
            services.AddDbContext<EventStoreContext>(option => option.UseSqlite(Configuration.GetConnectionString("EventStoreDatabase")));
            services.AddHangfire(configuration =>
                    GlobalConfiguration.Configuration.UseSQLiteStorage("Data Source=Hangfire.db;", options))
                .AddTransient<IEventStoreRepository, EventStoreRepository>()
                .AddTransient<IRowVersionRepository, RowVersionRepository>()
                .AddTransient<IUserRepository, UserRepository>()
                .AddTransient<OnUserCreateEventHandler>()
                .AddTransient<OnUserCreateEventAsynchronousHook>()
                .AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            var option = new BackgroundJobServerOptions {WorkerCount = 1};
            app.UseHangfireServer(option);
            app.UseHangfireDashboard();

            RecurringJob.AddOrUpdate<OnUserCreateEventHandler>(handler => handler.Run(), Cron.Minutely());

            app.UseMvc();
        }
    }
}