﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstractions;
using AssemblyMapping = Services.AssemblyReference;

namespace Services
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddAutoMapper(typeof(AssemblyMapping).Assembly);

            return services;
        }
    }
}
