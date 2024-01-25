﻿using Insurance.Api.Services.Insurance;
using Insurance.Api.Services.Surcharge;
using Microsoft.Extensions.DependencyInjection;

namespace Insurance.Api.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IInsuranceService, InsuranceService>();
            services.AddScoped<ISurchargeRateService, SurchargeRateService>();
            return services;
        }
    }
}
