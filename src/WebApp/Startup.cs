using ITOps.Middlewares;
using Marketing.Data;
using Marketing.ViewModelComposition;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sales.ViewModelComposition;
using ServiceComposer.AspNetCore;
using Shipping.ViewModelComposition;
using Warehouse.ViewModelComposition;

namespace WebApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; private set; }

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
            services.AddControllersWithViews();
            services.AddViewModelComposition(options =>
            {
                options.EnableCompositionOverControllers(true);
                options.EnableWriteSupport();
            });

            services.AddSingleton(new MarketingApi(this.Configuration.GetServiceUri("marketing-api")));
            services.AddSingleton(new SalesApi(this.Configuration.GetServiceUri("sales-api")));
            services.AddSingleton(new ShippingApi(this.Configuration.GetServiceUri("shipping-api")));
            services.AddSingleton(new WarehouseApi(this.Configuration.GetServiceUri("warehouse-api")));
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseBrowserLink();

            app.UseStaticFiles();
            app.UseMiddleware<ShoppingCartMiddleware>();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapCompositionHandlers();
            });
        }
    }
}
