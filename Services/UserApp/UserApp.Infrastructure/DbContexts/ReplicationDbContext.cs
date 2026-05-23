using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserApp.Infrastructure.DbContexts
{
    public class ReplicationDbContext : FollowMeIdentityDbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public ReplicationDbContext(
            DbContextOptions<ReplicationDbContext> options,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
            : base(options, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Ép toàn bộ query qua context này không được track để tối ưu slave
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            var region = _httpContextAccessor.HttpContext?.Request.Headers["X-User-Region"].ToString() ?? "VN";
            string configKey = region.ToUpper() switch
            {
                "US" => "ConnectionStrings:US:Replica",
                _ => "ConnectionStrings:VN:Replica"
            };
            string? connectionString = _configuration[configKey];
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException($"Không tìm thấy cấu hình Connection String cho key: '{configKey}'. Ný ơi kiểm tra lại appsettings.json nhé!");
            }
            optionsBuilder.UseSqlServer(connectionString);
            base.OnConfiguring(optionsBuilder);
        }

        // Chặn tuyệt đối việc ghi ở tầng Slave để bảo vệ dữ liệu Replication
        public override int SaveChanges()
            => throw new InvalidOperationException("Ný ơi, đây là DB Replica (Read-only), đừng ghi vào đây!");

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => throw new InvalidOperationException("Ný ơi, đây là DB Replica (Read-only), đừng ghi vào đây!");
    }
}

