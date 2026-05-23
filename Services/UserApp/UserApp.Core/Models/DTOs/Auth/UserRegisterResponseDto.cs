using FollowMe.Library.Core.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserApp.Core.Models.DTOs.Auth
{
    public class UserRegisterResponseDto
    {
        public bool IsSuccess { get; set; } = false;
        public NotiResponse Noti { get; set; } = new NotiResponse();
    }
}
