using Constructor.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Starcounter.Nova.Extensions.DependencyInjection;
using Starcounter.Palindrom;
using Starcounter.Palindrom.Database;

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
            services.AddSingleton<PropertyCrudManager>();
            services.AddMvc(o => o.EnableEndpointRouting = false);
            services.AddDatabaseInteractionContexts();
            services.AddPalindrom();

            // services.AddSingleton<IInteractionContext>(pr => pr.GetService<IStarcounterInteractionContext>());

            // The line above sets the Starcounter interaction context instance registered for the 
            // IStarcounterInteractionContext service interface as the instance of the general
            // IInteractionContext service, instead of IBasicInteractionContext. Doing so lets 
            // is assign a default interaction context to all Palindrom contexts.

            // To change the interaction context on a per-request basis, inject the interaction 
            // context in your controller, and assign it to the Palindrom context from there
            // (see ConstructorController).
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UsePalindrom();
            app.UseMvcWithDefaultRoute();
            Item.PropertyCrud = app.ApplicationServices.GetService<PropertyCrudManager>();
        }
    }
}