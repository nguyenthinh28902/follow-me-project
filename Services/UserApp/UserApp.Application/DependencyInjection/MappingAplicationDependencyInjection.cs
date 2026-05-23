using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserApp.Application.DependencyInjection
{
    public static class MappingAplicationDependencyInjection
    {
        public static IServiceCollection AddMappingAplication(this IServiceCollection services){
            services.AddAutoMapper(cfg => { 
            
                cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
            });
            return services;
        }
    }
}
