using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NS.Models;
using System.Linq;

namespace NS
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
            services.AddSingleton(this.Configuration);
            services.AddMvc(options => { options.EnableEndpointRouting = false; })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddOData();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.EntityType<VipCustomer>();
            modelBuilder.EntitySet<Order>("Orders");
            modelBuilder.EntitySet<Customer>("Customers");

            app.UseMvc(routeBuilder =>
            {
                routeBuilder.Filter().Expand().Select().OrderBy().MaxTop(null).SkipToken();
                routeBuilder.MapODataServiceRoute("odata", "odata", modelBuilder.GetEdmModel());
            });
        }
    }
}
