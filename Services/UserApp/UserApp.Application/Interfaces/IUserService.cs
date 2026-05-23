using System;
using System.Collections.Generic;
using System.Text;
using UserApp.Core.Models.DTOs.Auth;

namespace UserApp.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserRegisterResponseDto> UserRegisterAsync(UserRegisterDto userRegisterDto);
        Task<CheckUserResponseDto> CheckUserAsync(CheckUserRequestDto checkUserRequestDto);
    }
}
