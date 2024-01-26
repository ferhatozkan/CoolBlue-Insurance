using Insurance.Api.Application.Clients;
using Insurance.Api.Application.Repositories;
using Insurance.Api.Infrastructure.Clients;
using Insurance.Api.Infrastructure.Clients.Configuration;
using Insurance.Api.Infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Insurance.Api.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configurationManager)
        {
            services.Configure<ProductApiClientConfiguration>(options => configurationManager.GetSection("ProductApiClientConfiguration").Bind(options));
            services.AddScoped<IProductApiClient, ProductApiClient>();
            
            services.AddDbContextFactory<DataContext>();
            services.AddScoped<ISurchargeRateRepository, SurchargeRateRepository>();
            return services;
        }
    }
}
