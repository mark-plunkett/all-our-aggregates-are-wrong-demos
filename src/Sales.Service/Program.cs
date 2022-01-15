using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using Sales.Data;
using System;
using System.Threading.Tasks;

namespace Sales.Service
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceName = typeof(Program).Namespace;
            Console.Title = serviceName;

            var endpointConfig = new EndpointConfiguration(serviceName);
            endpointConfig.ApplyCommonConfiguration();
            endpointConfig.AuditSagaStateChanges(serviceControlQueue: "Particular.ServiceControl");

            var services = BuildServices();
            var endpoint = EndpointWithExternallyManagedServiceProvider.Create(endpointConfig, services);
            
            var endpointInstance = await endpoint.Start(new DefaultServiceProviderFactory().CreateServiceProvider(services));

            Console.WriteLine($"{serviceName} sarted. Press any key to stop.");
            Console.ReadLine();

            await endpointInstance.Stop();
        }

        static IServiceCollection BuildServices()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            var services = new ServiceCollection();
            services.RegisterSalesContext(config);
            return services;
        }
    }
}
