using Microsoft.Extensions.DependencyInjection;

namespace Insurance.Api.Repository
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddDbContextFactory<DataContext>();
            services.AddScoped<ISurchargeRateRepository, SurchargeRateRepository>();
            return services;
        }
    }
}
