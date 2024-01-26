using Insurance.Api.Application.Services.Insurance;
using Insurance.Api.Application.Services.Insurance.Chain;
using Insurance.Api.Application.Services.Insurance.Rules;
using Insurance.Api.Application.Services.Surcharge;
using Microsoft.Extensions.DependencyInjection;

namespace Insurance.Api.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IInsuranceService, InsuranceService>();
            services.AddScoped<ISurchargeRateService, SurchargeRateService>();

            // chain of responsibility pattern
            services.AddScoped<CanBeInsuredHandler>();
            services.AddScoped<PriceRuleInsuranceHandler>();
            services.AddScoped<SpecialTypeInsuranceHandler>();
            services.AddScoped<SurchargeRateHandler>();
            services.AddScoped<IInsuranceChainService, InsuranceChainService>();

            return services;
        }
    }
}
