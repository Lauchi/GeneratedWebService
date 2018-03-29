using System;
using Application.Users;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlAdapter;
using SqlAdapter.Users;

namespace AsyncHost
{
    public class Startup
    {
        private readonly OnUserCreateEventHandler _onUserCreateEventHandler;

        public Startup(IConfiguration configuration, OnUserCreateEventHandler onUserCreateEventHandler)
        {
            Configuration = configuration;
            _onUserCreateEventHandler = onUserCreateEventHandler;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var options = new SqlServerStorageOptions();
            services.AddHangfire(configuration =>
                GlobalConfiguration.Configuration.UseSqlServerStorage(
                    Configuration.GetConnectionString("HangfireDatabase"), options))
                .AddTransient<OnUserCreateEventHandler>()
                .AddTransient<EventStoreContext>()
                .AddTransient<IUserRepository, UserRepository>()
                .AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            var option = new BackgroundJobServerOptions {WorkerCount = 1};
            app.UseHangfireServer(option);
            app.UseHangfireDashboard();

            RecurringJob.AddOrUpdate(() => _onUserCreateEventHandler.Run(), Cron.Minutely);
            app.UseMvc();
        }
    }
}