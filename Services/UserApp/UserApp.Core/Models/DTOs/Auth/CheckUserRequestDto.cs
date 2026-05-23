using System;
using System.Collections.Generic;
using System.Text;

namespace UserApp.Core.Models.DTOs.Auth
{
    public class CheckUserRequestDto
    {
        public string UsernameOrEmail { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
