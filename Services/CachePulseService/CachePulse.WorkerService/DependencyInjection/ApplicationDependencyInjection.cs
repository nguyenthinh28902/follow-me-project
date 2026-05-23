using CachePulse.WorkerService.Interfaces;
using CachePulse.WorkerService.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace CachePulse.WorkerService.DependencyInjection
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Đăng ký các dịch vụ ứng dụng tại đây
            // Ví dụ: services.AddScoped<IMyService, MyService>();
            services.AddScoped<IBloomFilterService, BloomFilterService>();
            return services;
        }   
    }
}
