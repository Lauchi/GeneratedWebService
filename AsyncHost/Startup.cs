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
        private OnUserCreate onUserCreate;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var options = new SqlServerStorageOptions();
            services.AddHangfire(configuration =>
                GlobalConfiguration.Configuration.UseSqlServerStorage(
                    Configuration.GetConnectionString("HangfireDatabase"), options))
                .AddTransient<OnUserCreate>()
                .AddTransient<EventStoreContext>()
                .AddTransient<IUserRepository, UserRepository>()
                .AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            var option = new BackgroundJobServerOptions {WorkerCount = 1};
            app.UseHangfireServer(option);
            app.UseHangfireDashboard();

            RecurringJob.AddOrUpdate(() => onUserCreate.Run(), Cron.Minutely);
            app.UseMvc();
        }
    }
}