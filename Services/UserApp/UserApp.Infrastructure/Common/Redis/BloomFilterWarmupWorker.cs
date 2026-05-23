using FollowMe.Library.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UserApp.Application.Interfaces;
using UserApp.Core.Enums;
using UserApp.Infrastructure.DbContexts;

namespace UserApp.Infrastructure.Common.Redis
{
    public class BloomFilterWarmupWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IBloomFilterService _bloomFilterService;
        private readonly ILogger<BloomFilterWarmupWorker> _logger;
        public BloomFilterWarmupWorker(
            IServiceProvider serviceProvider,
            IBloomFilterService bloomFilterService,
            ILogger<BloomFilterWarmupWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _bloomFilterService = bloomFilterService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("🚀 Bắt đầu quá trình WARM-UP dữ liệu từ DB lên Redis Bloom Filter...");

            try
            {
                // 1. Khởi tạo cấu hình cho Filter trên Redis (Ví dụ chứa tối đa 500,000 dòng dữ liệu)
                await _bloomFilterService.InitFilterAsync(FilterRedis.FilterEmail, capacity: 500000, errorRate: 0.01);
                await _bloomFilterService.InitFilterAsync(FilterRedis.FilterUsername, capacity: 500000, errorRate: 0.01);

                // 2. Vì BackgroundService là Singleton nên muốn gọi DbContext (Scoped) phải tạo Scope riêng
                using (var scope = _serviceProvider.CreateScope())
                {
                    // Thay cái 'YourDbContext' bằng tên DbContext thực tế của dự án mày
                    var dbContext = scope.ServiceProvider.GetRequiredService<ReplicationDbContext>();

                    // Lấy cả UserName và Email ra (chỉ lấy 2 cột này để nhẹ RAM)
                    var allUsers = await dbContext.GlobalIdentifiers
                        .Select(g => new { g.Email, g.UserName })
                        .AsNoTracking()
                        .ToListAsync(stoppingToken);

                    _logger.LogInformation($"Tìm thấy {allUsers.Count} tài khoản trong DB để nạp vào Cache.");

                    foreach (var user in allUsers)
                    {
                        if (stoppingToken.IsCancellationRequested) break;

                        // Nạp email vào bộ lọc email, username vào bộ lọc username
                        await _bloomFilterService.AddAsync(FilterRedis.FilterEmail, user.Email);
                        await _bloomFilterService.AddAsync(FilterRedis.FilterUsername, user.UserName);
                    }

                    _logger.LogInformation("✅ Hoàn thành warm-up cho cả Email và UserName!");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Gặp lỗi chí mạng khi đang nạp dữ liệu lên Bloom Filter!");
            }
        }
    }
}
