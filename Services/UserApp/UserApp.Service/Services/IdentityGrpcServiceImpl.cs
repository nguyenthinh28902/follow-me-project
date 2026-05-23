using Grpc.Core;
using UserApp.Application.Interfaces;
using UserApp.Core.Models.DTOs.Auth;
using UserApp.Service.Protos;
namespace UserApp.Service.Services
{
    public class IdentityGrpcServiceImpl : IdentityGrpcService.IdentityGrpcServiceBase
    {
        private readonly IUserService _userService;

        public IdentityGrpcServiceImpl(IUserService userService)
        {
            _userService = userService;
        }

        public override async Task<CheckUserGrpcResponse> CheckUser(CheckUserGrpcRequest request, ServerCallContext context)
        {
            var dtoRequest = new CheckUserRequestDto
            {
                UsernameOrEmail = request.UsernameOrEmail,
                Password = request.Password
            };

            var result = await _userService.CheckUserAsync(dtoRequest);

            var response = new CheckUserGrpcResponse
            {
                IsValid = result.IsValid,
                UserId = result.UserId ?? string.Empty,
                Noti = MapToGrpcNoti(result.Noti) // Map cấu trúc NotiResponse tĩnh của hệ thống sang protobuf object
            };

            return response;
        }

        public override async Task<UserRegisterGrpcResponse> UserRegister(UserRegisterGrpcRequest request, ServerCallContext context)
        {
            var dtoRequest = new UserRegisterDto
            {
                UserName = request.Username,
                Email = request.Email,
                Password = request.Password,
                FullName = request.FullName,
                PhoneNumber = string.IsNullOrEmpty(request.PhoneNumber) ? null : request.PhoneNumber,
                Region = request.Region
            };

            var result = await _userService.UserRegisterAsync(dtoRequest);

            var response = new UserRegisterGrpcResponse
            {
                IsSuccess = result.IsSuccess,

                Noti = MapToGrpcNoti(result.Noti)
            };

            return response;
        }

        /// <summary>
        /// Bộ chuyển đổi dữ liệu thông báo/lỗi có cấu trúc từ hệ thống nội bộ sang định dạng gRPC tương thích.
        /// </summary>
        private static GrpcNotiResponse MapToGrpcNoti(FollowMe.Library.Core.Constants.NotiResponse internalNoti)
        {
            if (internalNoti == null) return new GrpcNotiResponse();

            var grpcNoti = new GrpcNotiResponse
            {
                Code = internalNoti.Code ?? string.Empty,
                MessageKey = internalNoti.MessageKey ?? string.Empty
            };

            // Ép mảng lỗi chi tiết Validation nếu có dữ liệu đính kèm từ tầng Service
            if (internalNoti.Details != null && internalNoti.Details.Any())
            {
                foreach (var detail in internalNoti.Details)
                {
                    grpcNoti.Details.Add(new GrpcValidationDetail
                    {
                        Field = detail.Field ?? string.Empty,
                        Code = detail.Code ?? string.Empty,
                        MessageKey = detail.MessageKey ?? string.Empty
                    });
                }
            }

            return grpcNoti;
        }
    }
}
