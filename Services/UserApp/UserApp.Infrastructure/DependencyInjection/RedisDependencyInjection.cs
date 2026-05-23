using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using UserApp.Application.Interfaces;
using UserApp.Infrastructure.Common.Redis;
using UserApp.Infrastructure.Services;

namespace UserApp.Infrastructure.DependencyInjection
{
    public static class RedisDependencyInjection
    {
        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConnectionString = configuration.GetConnectionString("Redis") ?? "localhost:6379";
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));
            services.AddSingleton<IBloomFilterService, BloomFilterService>();
            services.AddHostedService<BloomFilterWarmupWorker>();
            return services;
        }
    }
}
