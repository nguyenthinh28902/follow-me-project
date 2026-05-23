using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using UserApp.Core.Entities;

namespace UserApp.Infrastructure.DbContexts
{
    public class FollowMeIdentityDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly IConfiguration _configuration;


        // Hàm khởi tạo chính dùng cho FollowMeIdentityDbContext
        public FollowMeIdentityDbContext(
            DbContextOptions<FollowMeIdentityDbContext> options,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        // Hàm khởi tạo protected giúp class con (ReplicationDbContext) kế thừa cấu trúc mượt mà
        protected FollowMeIdentityDbContext(
            DbContextOptions options,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public DbSet<RegionConfig> Regions { get; set; }
        public DbSet<BusinessProfile> BusinessProfiles { get; set; }
        public DbSet<BusinessProfileHistory> BusinessProfileHistories { get; set; }
        public DbSet<GlobalIdentifier> GlobalIdentifiers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // 1. Nhận diện vùng của User từ Gateway hoặc Cloudflare header, mặc định là VN
            var region = _httpContextAccessor.HttpContext?.Request.Headers["X-User-Region"].ToString() ?? "US";

            // 2. Xác định Key cấu hình dựa trên vùng (Region) nhằm xử lý Master ghi dữ liệu
            string configKey = region.ToUpper() switch
            {
                "US" => "ConnectionStrings:US:Master",
                _ => "ConnectionStrings:VN:Master"
            };

            // 3. Lấy chuỗi kết nối an toàn và kiểm tra null để loại bỏ hoàn toàn gạch cảnh báo xanh
            string connectionString = _configuration[configKey]
                ?? throw new InvalidOperationException($"cấu hình {configKey} đang bị trống trong appsettings.json!");

            optionsBuilder.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            });

            base.OnConfiguring(optionsBuilder);
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 1. Cấu hình bảng RegionConfig
            builder.Entity<RegionConfig>(entity =>
            {
                entity.ToTable("RegionConfigs");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.RegionCode).IsUnique();
            });

            // 2. Cấu hình bảng AppUser với khóa ngoại Vùng miền
            builder.Entity<AppUser>(entity =>
            {
                entity.HasOne(u => u.HomeRegion)
                      .WithMany(r => r.HomeUsers)
                      .HasForeignKey(u => u.HomeRegionId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(u => u.CurrentRegion)
                      .WithMany(r => r.CurrentUsers)
                      .HasForeignKey(u => u.CurrentRegionId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // 3. ĐÃ CẬP NHẬT: Cấu hình quan hệ 1 - 1 giữa AppUser và BusinessProfile
            builder.Entity<BusinessProfile>(entity =>
            {
                entity.ToTable("BusinessProfiles");
                entity.HasKey(e => e.Id);

                entity.HasOne(p => p.AppUser)
                      .WithOne(u => u.BusinessProfile)
                      .HasForeignKey<BusinessProfile>(p => p.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // 4. ĐÃ CẬP NHẬT: Cấu hình bảng Lịch Sử BusinessProfileHistory
            builder.Entity<BusinessProfileHistory>(entity =>
            {
                entity.ToTable("BusinessProfileHistories");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserId); // Tạo Index để phục vụ truy vấn lịch sử siêu tốc
            });
        }
    }
}
