using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shipping.Data
{
    public static class Extensions
    {
        public static void RegisterShippingContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ShippingContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("sqlserver", "shipping"));
                using var context = new ShippingContext(options.Options);
                context.Database.EnsureCreated();
            });
        }
    }
}
