using System;
using System.Collections.Generic;
using System.Text;
using UserApp.Core.Enums;

namespace UserApp.Core.Entities
{
    public class BusinessProfileHistory
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();

        // Chỉ lưu ID của User để tra cứu lịch sử nhanh, không dùng Cascade Delete để bảo vệ tính toàn vẹn của Log
        public Guid UserId { get; set; }

        // --- SNAPSHOT DỮ LIỆU TẠI THỜI ĐIỂM HÀNH ĐỘNG ---
        public string DisplayName { get; set; } = null!;
        public string? TaxOrIdentityCode { get; set; }
        public string? ContactEmail { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? Description { get; set; }

        // --- TRẠNG THÁI GHI NHẬN LÚC ĐÓ ---
        public ApprovalStatus LoggedApprovalStatus { get; set; }

        // Ghi chú hành động (Ví dụ: "User cập nhật thông tin", "Admin X phê duyệt")
        public string? Note { get; set; }

        // Người thực hiện hành động (ID của User hoặc ID của Admin)
        public string ActionBy { get; set; } = null!;
        public DateTime ActionAt { get; set; } = DateTime.UtcNow;
    }
}
