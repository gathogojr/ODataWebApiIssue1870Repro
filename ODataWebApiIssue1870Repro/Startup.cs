using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;
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
            IEdmModel model = modelBuilder.GetEdmModel();
            Test(model);

            app.UseMvc(routeBuilder =>
            {
                routeBuilder.Filter().Expand().Select().OrderBy().MaxTop(null).SkipToken();
                routeBuilder.MapODataServiceRoute("odata", "odata", model);
            });
        }

        private static void Test(IEdmModel model)
        {
            IEdmEntityType order = model.SchemaElements.OfType<IEdmEntityType>().First(c => c.Name == "Order");
            IEdmEntityType customer = model.SchemaElements.OfType<IEdmEntityType>().First(c => c.Name == "Customer");
            IEdmEntityType vipCustomer = model.SchemaElements.OfType<IEdmEntityType>().First(c => c.Name == "VipCustomer");

            IEdmNavigationProperty navProp = order.FindProperty("Customer") as IEdmNavigationProperty;
            IEdmEntitySet customers = model.EntityContainer.FindEntitySet("Customers");
            IEdmEntitySet orders = model.EntityContainer.FindEntitySet("Orders");
            ODataPathSegment navSegment = new NavigationPropertySegment(navProp, orders);
            ODataPathSegment typeSegment = new TypeSegment(vipCustomer, customer, customers);

            // this throw: The last segment, and only the last segment, must be a navigation property in $expand.
            // ODataExpandPath path = new ODataExpandPath(navSegment, typeSegment);
        }
    }
}
