using AutoMapper;
using FollowMe.Library.Core.Repository.Abstractions.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UserApp.Application.Interfaces;
using UserApp.Core.Entities;
using UserApp.Core.Enums;
using UserApp.Core.Models.DTOs.Auth;

namespace UserApp.Infrastructure.Services
{
    public class UserIdentityService : IUserIdentityService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;


        public UserIdentityService(UserManager<AppUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CheckUserCredentialsAsync(AppUser user, string password)
        {
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
            return isPasswordValid;
        }

        public async Task<AppUser?> UserAsync(string usernameOrEmail)
        {
            var user = usernameOrEmail.Contains("@")
               ? await _userManager.Users
                   .Include(u => u.HomeRegion)
                   .Include(u => u.CurrentRegion)
                   .Include(u => u.BusinessProfile)
                   .FirstOrDefaultAsync(u => u.Email == usernameOrEmail && !u.IsDeleted)
               : await _userManager.Users
                   .Include(u => u.HomeRegion)
                   .Include(u => u.CurrentRegion)
                   .Include(u => u.BusinessProfile)
                   .FirstOrDefaultAsync(u => u.UserName == usernameOrEmail && !u.IsDeleted);
            return user;
        }



        public async Task<bool> RegisterAsync(AppUser user, string password, string RegionCode)
        {
            var userRegisterResponseDto = new UserRegisterResponseDto();
            var existingUser = await _userManager.FindByEmailAsync(user.Email ?? string.Empty)
                               ?? await _userManager.FindByNameAsync(user.UserName ?? string.Empty);
            if (existingUser != null)
                return false;
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var result = await _userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    await _unitOfWork.RollbackAsync();
                    return false;
                }
                var globalIdentifier = new GlobalIdentifier
                {
                    Id = user.Id,
                    UserName = user.UserName!,
                    Email = user.Email!,
                    RegionCode = RegionCode,
                    CreatedAt = DateTime.UtcNow
                };
                await _unitOfWork.Repository<GlobalIdentifier>().AddAsync(globalIdentifier);
                await _unitOfWork.CommitAsync();
                return result.Succeeded;
            }
            catch (Exception ex)
            {
               await _unitOfWork.RollbackAsync(); 
               throw;
            }
        }

        public Task<IList<string>> GetUserRolesAsync(AppUser user)
        => _userManager.GetRolesAsync(user);
    }
}
