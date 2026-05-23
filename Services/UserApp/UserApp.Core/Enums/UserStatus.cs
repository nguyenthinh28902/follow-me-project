using System;
using System.Collections.Generic;
using System.Text;

namespace UserApp.Core.Enums
{
    public enum UserStatus
    {
        Suspended = 0, // Tài khoản bị khóa / tạm ngưng hoạt động
        Active = 1,    // Tài khoản đang hoạt động bình thường
        Banned = 2     // Tài khoản bị ban vĩnh viễn khỏi hệ thống
    }
}
