using FluentValidation;
using Insurance.Api.Models.Request;
using Microsoft.Extensions.DependencyInjection;

namespace Insurance.Api.Validators
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<UpdateSurchargeRateRequest>, UpdateSurchargeRateRequestValidator>();
            services.AddScoped<IValidator<CreateSurchargeRateRequest>, CreateSurchargeRateRequestValidator>();
            return services;
        }
    }
}
