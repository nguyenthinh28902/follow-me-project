using System;
using System.Collections.Generic;
using System.Text;
using UserApp.Core.Enums;

namespace UserApp.Core.Entities
{
    public class BusinessProfile
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();

        // Khóa ngoại nối sang AspNetUsers (Quan hệ 1-1)
        public Guid UserId { get; set; }

        // --- THÔNG TIN CHI TIẾT CỦA BUSINESS / BLOG ---
        public string DisplayName { get; set; } = null!;      // Tên doanh nghiệp hoặc Tên hiển thị của Blog/KOL
        public string? TaxOrIdentityCode { get; set; }        // Mã số thuế (Business) hoặc Số CCCD/Hộ chiếu (Blog)
        public string? ContactEmail { get; set; }             // UsernameOrEmail nhận liên hệ hợp tác/công việc
        public string? WebsiteUrl { get; set; }
        public string? Description { get; set; }              // Giới thiệu ngắn về doanh nghiệp/chủ đề blog

        // --- TIẾN TRÌNH PHÊ DUYỆT CỦA PROFILE ---
        public ApprovalStatus ApprovalStatus { get; set; } = ApprovalStatus.Pending;
        public string? RejectReason { get; set; } // Lý do từ chối nếu ApprovalStatus = Rejected

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property liên kết ngược về AppUser
        public virtual AppUser AppUser { get; set; } = null!;
    }
}
