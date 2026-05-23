using System;
using System.Collections.Generic;
using System.Text;

namespace UserApp.Core.Enums
{
    public enum ApprovalStatus
    {
        Pending = 0,    // Chờ duyệt
        Approved = 1,   // Đã duyệt (Thông tin này chính thức có hiệu lực và đang hiển thị)
        Rejected = 2    // Bị từ chối
    }
}
