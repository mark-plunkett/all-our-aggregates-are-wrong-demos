using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Warehouse.Data
{
    public static class Extensions
    {
        public static void RegisterWarehouseContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<WarehouseContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("sqlserver", "warehouse"));
                using var context = new WarehouseContext(options.Options);
                context.Database.EnsureCreated();
            });
        }
    }
}
