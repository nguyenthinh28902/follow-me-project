using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UserApp.Core.Entities;
using UserApp.Core.Enums;
using UserApp.Infrastructure.DbContexts;

namespace UserApp.Infrastructure.Data
{
    public static class DbInitializerExtension
    {
        public static async Task SeedSystemRegions(this IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<FollowMeIdentityDbContext>();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            try
            {
                context.Database.Migrate();

                // Nếu bảng đã có dữ liệu, bỏ qua không seed nữa để tránh ghi đè dữ liệu vận hành
                if (context.Regions.Any()) return;

                var regions = new[]
                {
                new RegionConfig
                {
                    Id = Guid.CreateVersion7(), // Tối ưu Index phân tán
                    RegionCode = RegionType.VietNam.ToString().ToUpper(), // "VIETNAM"
                    RegionName = "Việt Nam (Cụm Đông Nam Á)",
                    InternalRpcEndpoint = "https://rpc-vn.yourdomain.internal:5001", // Endpoint gRPC nội bộ phục vụ sync data
                    Status = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new RegionConfig
                {
                    Id = Guid.CreateVersion7(),
                    RegionCode = RegionType.UnitedStates.ToString().ToUpper(), // "UNITEDSTATES"
                    RegionName = "Mỹ (Cụm Bắc Mỹ)",
                    InternalRpcEndpoint = "https://rpc-us.yourdomain.internal:5001",
                    Status = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

                context.Regions.AddRange(regions);

                // Ép buộc lưu dữ liệu vào Master DB thông qua hàm SaveChanges đã override
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger("InfrastructureExtensions");
                logger.LogError(ex, "Lỗi xảy ra trong quá trình Seed Data hệ thống.");
                throw; 
            }
            
        }
    }
}
