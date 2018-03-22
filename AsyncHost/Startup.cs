using System;
using System.Diagnostics;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            var options = new SqlServerStorageOptions();
            services.AddHangfire(configuration =>
                GlobalConfiguration.Configuration.UseSqlServerStorage(
                    Configuration.GetConnectionString("HangfireDatabase"), options)).AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            var option = new BackgroundJobServerOptions {WorkerCount = 1};
            app.UseHangfireServer(option);
            app.UseHangfireDashboard();

            RecurringJob.AddOrUpdate(() => Run(), Cron.Minutely);
            app.UseMvc();
        }

        public void Run()
        {
            Debug.WriteLine($"Run at {DateTime.Now}");
        }
    }
}