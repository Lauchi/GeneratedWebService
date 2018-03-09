using Application;
using Application.Users;
using HttpAdapter.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlAdapter;
using SqlAdapter.Users;

namespace GeneratedWebService
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
            services.AddDbContext<EventStoreContext>(option => option.UseSqlite("Data Source=Eventstore.db"))
                .AddTransient<IEventStore, EventStore>()
                .AddTransient<IEventStoreRepository, EventStoreRepository>()
                .AddTransient<IUserRepository, UserRepository>()
                .AddTransient<IUserCommandHandler, UserCommandHandler>()
                .AddMvc()
                .AddApplicationPart(typeof(UserController).Assembly);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc();
        }
    }
}
