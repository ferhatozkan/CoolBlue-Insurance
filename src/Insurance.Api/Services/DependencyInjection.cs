using Insurance.Api.Services.Insurance;
using Microsoft.Extensions.DependencyInjection;

namespace Insurance.Api.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IInsuranceService, InsuranceService>();
            return services;
        }
    }
}
