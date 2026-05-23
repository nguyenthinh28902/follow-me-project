using FollowMe.Library.Core.Repository.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserApp.Application.Interfaces;
using UserApp.Core.Entities;
using UserApp.Infrastructure.DbContexts;
using UserApp.Infrastructure.Repositories;
using UserApp.Infrastructure.Services;

namespace UserApp.Infrastructure.DependencyInjection
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<FollowMeIdentityDbContext>();
            services.AddDbContext<ReplicationDbContext>();

            services.AddIdentityCore<AppUser>(options =>
            {
                // Mày có thể cấu hình các điều kiện bảo mật mật khẩu tại đây
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 6;

                // Cấu hình không cho phép trùng UsernameOrEmail
                options.User.RequireUniqueEmail = true;
            }).AddRoles<AppRole>()
            .AddEntityFrameworkStores<FollowMeIdentityDbContext>();
            //repo & uow
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IReplicationUnitOfWork, ReplicationUnitOfWork>(sp => {
                var context = sp.GetRequiredService<ReplicationDbContext>();
                var serviceProvider = sp.GetRequiredService<IServiceProvider>();
                return new ReplicationUnitOfWork(context, serviceProvider); // UnitOfWork này sẽ dùng ReplicaDb
            });

            services.AddScoped<IUserIdentityService, UserIdentityService>();
            services.AddScoped<IRegionConfigRepository, RegionConfigRepository>();
            return services;
        }
    }
}
