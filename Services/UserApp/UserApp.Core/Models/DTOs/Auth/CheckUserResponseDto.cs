using FollowMe.Library.Core.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserApp.Core.Models.DTOs.Auth
{
    public class CheckUserResponseDto
    {
        public bool IsValid { get; set; } = false;
        public string? UserId { get; set; }
        public NotiResponse Noti { get; set; } = new NotiResponse();
    }
}
