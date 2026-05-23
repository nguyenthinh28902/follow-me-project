using System;
using System.Collections.Generic;
using System.Text;
using UserApp.Core.Entities;
using UserApp.Core.Models.DTOs.Auth;

namespace UserApp.Application.Interfaces
{
    public interface IUserIdentityService
    {
        Task<bool> RegisterAsync(AppUser user, string password, string RegionCode);
        Task<bool> CheckUserCredentialsAsync(AppUser user, string password);
        Task<AppUser?> UserAsync(string usernameOrEmail);
        Task<IList<string>> GetUserRolesAsync(AppUser user);
    }
}
