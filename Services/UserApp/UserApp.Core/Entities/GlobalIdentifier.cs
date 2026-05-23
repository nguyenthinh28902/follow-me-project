using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Xml.Linq;

namespace UserApp.Core.Entities
{
    [Index(nameof(Email), IsUnique = true, Name = "IX_GlobalIdentifier_Email")]
    [Index(nameof(UserName), IsUnique = true, Name = "IX_GlobalIdentifier_UserName")]
    public class GlobalIdentifier
    {
        [Key]
        public Guid Id { get; set; } = Guid.CreateVersion7();

        [Required]
        [MaxLength(150)] // Giới hạn thay vì NVARCHAR(MAX)
        public string UserName { get; set; } = null!;

        [Required]
        [MaxLength(256)] // Chuẩn độ dài Email quốc tế, giúp đánh Index tốt
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(10)] // Giới hạn mã vùng (VN, US, EU...)
        [Column(TypeName = "varchar(10)")] // Ép hẳn về kiểu varchar để tiết kiệm bộ nhớ (không cần unicode)
        public string RegionCode { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Mặc định lấy giờ UTC luôn cho chuẩn phân tán
    }
}
