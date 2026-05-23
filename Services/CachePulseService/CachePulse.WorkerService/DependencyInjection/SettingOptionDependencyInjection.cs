using CachePulse.WorkerService.Models.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace CachePulse.WorkerService.DependencyInjection
{
    public static class SettingOptionDependencyInjection
    {
        public static IServiceCollection AddSettingOption(this IServiceCollection services, IConfiguration configuration)
        {
            // Đăng ký các lớp cấu hình nếu cần thiết
            // Ví dụ: services.Configure<MySettings>(configuration.GetSection("MySettings"));
            services.Configure<KafkaSettings>(configuration.GetSection("KafkaSettings"));
            return services;
        }
    }
}
