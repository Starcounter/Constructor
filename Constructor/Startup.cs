using Constructor.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Starcounter.Nova.Extensions.DependencyInjection;
using Starcounter.XSON.Palindrom.AspNetCore;

namespace Constructor
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
            services.AddResponseCaching();
            services.AddStarcounter("Database=./.database/Constructor");
            services.Configure<KestrelServerOptions>(o => o.AllowSynchronousIO = true);
            services.AddSingleton<PropertyCrudManager>();
            services.AddMvc(o => o.EnableEndpointRouting = false);
            services.AddXSON("Constructor", "__constructor");
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseWebSockets();
            app.UseXSON();
            app.UseMvcWithDefaultRoute();
            Item.PropertyCrud = app.ApplicationServices.GetService<PropertyCrudManager>();
        }
    }
}