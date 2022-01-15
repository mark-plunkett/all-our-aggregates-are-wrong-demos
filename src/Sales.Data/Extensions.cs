using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Sales.Data
{
    public static class Extensions
    {
        public static void RegisterSalesContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SalesContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("sqlserver", "sales"));
                using var context = new SalesContext(options.Options);
                context.Database.EnsureCreated();
            });
        }
    }
}
