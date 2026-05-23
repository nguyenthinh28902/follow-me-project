using System;
using System.Collections.Generic;
using System.Text;

namespace UserApp.Core.Models.DTOs.Auth
{
    public class UserRegisterDto
    {
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string Region { get; set; } = null!;
    }
}
