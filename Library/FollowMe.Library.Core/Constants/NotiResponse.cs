using System;
using System.Collections.Generic;
using System.Text;

namespace FollowMe.Library.Core.Constants
{
    public class NotiResponse
    {
        public string Code { get; set; } = null!;
        public string MessageKey { get; set; } = null!;
        public List<ValidationNotiDetail>? Details { get; set; }
    }

    public class ValidationNotiDetail
    {
        public string Field { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string MessageKey { get; set; } = null!;
    }
    public class ModelNoti
    {
        public ModelNoti(string code, string messageKey)
        {
            Code = code;
            MessageKey = messageKey;
        }

        public string Code { get; } // Bỏ set để biến đối tượng thành Immutable (không thể sửa đổi sau khi tạo)
        public string MessageKey { get; }
    }
}
