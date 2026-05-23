using CachePulse.WorkerService.Interfaces;
using CachePulse.WorkerService.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace CachePulse.WorkerService.DependencyInjection
{
    public static class RedisDependencyInjection
    {
        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConnectionString = configuration["RedisConnection"] ?? "localhost:6379";
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));
            services.AddSingleton<IBloomFilterService, BloomFilterService>();
           
            return services;
        }
    }
}
