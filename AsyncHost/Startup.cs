using Hangfire;
using Hangfire.SQLite;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlAdapter;

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
            GeneratedDependencies.ConfigureGeneratedServices(services);
            var options = new SQLiteStorageOptions();
            services.AddDbContext<EventStoreContext>(option => option.UseSqlite(Configuration.GetConnectionString("EventStoreDatabase")));
            services.AddDbContext<HangfireContext>(option => option.UseSqlite(Configuration.GetConnectionString("HangfireDatabase")));
            services.AddHangfire(configuration =>
                    GlobalConfiguration.Configuration.UseSQLiteStorage(Configuration.GetConnectionString("HangfireDatabase"), options))
                .AddMvc();
            
        }

        public void Configure(IApplicationBuilder app)
        {
            GeneratedDependencies.ConfigureApplication(app);
            app.UseMvc();
        }
    }
}