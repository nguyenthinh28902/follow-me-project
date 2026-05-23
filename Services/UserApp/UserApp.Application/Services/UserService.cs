using AutoMapper;
using FollowMe.Library.Core.Constants;
using Microsoft.Extensions.Logging;
using UserApp.Application.Interfaces;
using UserApp.Core.Constants;
using UserApp.Core.Entities;
using UserApp.Core.Models.DTOs.Auth;

namespace UserApp.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserIdentityService _userIdentityService;
        private readonly IRegionConfigRepository _regionConfigRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        public UserService(IUserIdentityService userIdentityService, IRegionConfigRepository regionConfigRepository, IMapper mapper, ILogger<UserService> logger)
        {
            _userIdentityService = userIdentityService;
            _regionConfigRepository = regionConfigRepository;
            _mapper = mapper;
            _logger = logger;

        }

        /// <summary>
        /// Đăng  lý thuần tài khoản thông thường
        /// </summary>
        /// <param name="userRegisterDto">thông tin đăng ký nhập từ người dùng</param>
        /// <returns></returns>
        public async Task<UserRegisterResponseDto> UserRegisterAsync(UserRegisterDto userRegisterDto)
        {
            var response = new UserRegisterResponseDto();

            // Kiểm tra tính hợp lệ của mã vùng đầu vào để gán chính xác cụm DB gốc lưu trữ cho User
            var regionConfig = await _regionConfigRepository.GetRegionConfigAsync(userRegisterDto.Region);
            if (regionConfig == null)
            {
                response.Noti.Code = AuthNotiKeys.RegionNotFound.Code;
                response.Noti.MessageKey = AuthNotiKeys.RegionNotFound.MessageKey;
                return response;
            }

            var appUser = _mapper.Map<AppUser>(userRegisterDto);

            // Ép cứng định danh vùng gốc (Home) và vùng kết nối hiện tại (Current) của User lúc khởi tạo
            appUser.HomeRegionId = regionConfig.Id;
            appUser.CurrentRegionId = regionConfig.Id;

            try
            {
                response.IsSuccess = await _userIdentityService.RegisterAsync(appUser, userRegisterDto.Password, regionConfig.RegionCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return response;
        }

        /// <summary>
        /// Xác thực thông tin tài khoản và trả về thông báo trạng thái dạng DTO chuẩn cho client.
        /// </summary>
        public async Task<CheckUserResponseDto> CheckUserAsync(CheckUserRequestDto checkUserRequestDto)
        {
            var noti = new NotiResponse();
            var user = await _userIdentityService.UserAsync(checkUserRequestDto.UsernameOrEmail);

            if (user == null)
            {
                noti = new NotiResponse() { Code = AuthNotiKeys.UserNotFound.Code, MessageKey = AuthNotiKeys.UserNotFound.MessageKey };

                return new CheckUserResponseDto
                {
                    Noti = noti
                };
            }
            else if (user.LockoutEnabled)
            {
                noti = new NotiResponse() { Code = AuthNotiKeys.AccountLocked.Code, MessageKey = AuthNotiKeys.AccountLocked.MessageKey };
                return new CheckUserResponseDto
                {
                    Noti = noti
                };
            }

            var isCredentials = await _userIdentityService.CheckUserCredentialsAsync(user, checkUserRequestDto.Password);
            noti = isCredentials
                ? new NotiResponse() { Code = AuthNotiKeys.CredentialsValidated.Code, MessageKey = AuthNotiKeys.CredentialsValidated.MessageKey }
                : new NotiResponse()
                {
                    Code = AuthNotiKeys.InvalidCredentials.Code
                    ,
                    MessageKey = AuthNotiKeys.InvalidCredentials.MessageKey
                    ,
                    Details = new List<ValidationNotiDetail>()
                        {
                            new ValidationNotiDetail()
                                {
                                    Field = nameof(CheckUserRequestDto.Password),
                                    Code =  AuthNotiKeys.InvalidPassword.Code,
                                    MessageKey = AuthNotiKeys.InvalidPassword.MessageKey
                                }
                        }

                };

            return new CheckUserResponseDto
            {
                IsValid = isCredentials,
                Noti = noti,
                UserId = user.Id.ToString()
            };
        }
    }
}
