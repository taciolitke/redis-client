using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RedisClient.Service;
using RedisClient.Service.Interface;
using System;
using System.IO;

namespace RedisClient.IoC
{
    public static class DependencyResolve
    {
        public static ServiceProvider Resolve()
        {
            var serviceProvider = new ServiceCollection()
                    .ConfigureServices()
                    .AddCachedServices()
                    .AddConfigurationBuilder();

            return serviceProvider
                .BuildServiceProvider();
        }

        private static IServiceCollection ConfigureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddScoped<ICacheService, CacheService>();

            return serviceCollection;
        }

        private static IServiceCollection AddCachedServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDistributedRedisCache(options =>
            {
                options.Configuration = ConfigurationBuilder().GetConnectionString("RedisCluster");
                options.InstanceName = "RedisCluster";
            });

            return serviceCollection;
        }

        private static IServiceCollection AddConfigurationBuilder(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddSingleton(ConfigurationBuilder());
        }

        private static IConfigurationRoot ConfigurationBuilder()
        {
            return new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                .Build();
        }
    }
}
