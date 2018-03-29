using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlAdapter;

namespace Host
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
            services.AddDbContext<EventStoreContext>(option => option.UseSqlite(Configuration.GetConnectionString("EventStoreDatabase")));
            services.AddDbContext<HangfireContext>(option => option.UseSqlite(Configuration.GetConnectionString("HangfireDatabase")));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc();
        }
    }
}
