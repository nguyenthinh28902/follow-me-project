using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using UserApp.Core.Enums;

namespace UserApp.Core.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FullName { get; set; } = null!;
        public UserStatus Status { get; set; } = UserStatus.Active;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;

        public Guid HomeRegionId { get; set; }
        public Guid CurrentRegionId { get; set; }

        public virtual RegionConfig HomeRegion { get; set; } = null!;
        public virtual RegionConfig CurrentRegion { get; set; } = null!;

        // ĐÃ ĐỔI TÊN: Quan hệ 1 - 1 sang bảng thông tin chi tiết Business/Blog
        public virtual BusinessProfile? BusinessProfile { get; set; }
    }
}
