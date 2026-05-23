using System;
using System.Collections.Generic;
using System.Text;

namespace UserApp.Core.Entities
{
    public class RegionConfig
    {
        // Khóa chính dùng Guid tuần tự để tối ưu Index
        public Guid Id { get; set; } = Guid.CreateVersion7();

        // Mã vùng ngắn gọn (Ví dụ: "SEA", "US_EAST", "EU_WEST") -> Sẽ cấu hình UNIQUE INDEX
        public string RegionCode { get; set; } = null!;

        // Tên hiển thị trực quan (Ví dụ: "Đông Nam Á", "Đông Mỹ")
        public string RegionName { get; set; } = null!;

        // Địa chỉ Endpoint nội bộ của cụm gRPC/Message Broker tại vùng đó (Phục vụ đồng bộ xuyên vùng)
        public string InternalRpcEndpoint { get; set; } = null!;

        // Trạng thái cụm Server vùng (1: Active, 0: Bảo trì/Sập)
        public int Status { get; set; } = 1;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // --- NAVIGATION PROPERTIES ---
        // Các User thuộc về vùng gốc này (Nơi lưu DB chính)
        public virtual ICollection<AppUser> HomeUsers { get; set; } = new List<AppUser>();

        // Các User hiện đang vãng lai hoặc kết nối tại vùng này
        public virtual ICollection<AppUser> CurrentUsers { get; set; } = new List<AppUser>();
    }
}
