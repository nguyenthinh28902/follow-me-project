using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using UserApp.Core.Entities;
using UserApp.Core.Enums;
using UserApp.Core.Models.DTOs.Auth;

namespace UserApp.Application.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            // Ví dụ: Map từ Request DTO sang IdentityUser của .NET
            CreateMap<UserRegisterDto, AppUser>()
             .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))

             .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
             .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
             // Tạm thời bỏ qua 2 trường ID Region để xử lý thủ công bằng ID tìm từ DB
             .ForMember(dest => dest.HomeRegionId, opt => opt.Ignore())
             .ForMember(dest => dest.CurrentRegionId, opt => opt.Ignore())
             // Khởi tạo các trường mặc định nếu cần
             .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
             .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
             .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => UserStatus.Active))
             .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(_ => false));

            // Nếu ông có Entity User cục bộ hoặc Profile, cấu hình tiếp tại đây
            // CreateMap<UserEntity, UserDto>().ReverseMap();
        }
    }
}
