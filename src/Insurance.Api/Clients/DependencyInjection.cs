﻿using Insurance.Api.Clients.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Insurance.Api.Clients
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddClients(this IServiceCollection services, IConfiguration configurationManager) 
        {
            services.Configure<ProductApiClientConfiguration>(options => configurationManager.GetSection("ProductApiClientConfiguration").Bind(options));
            services.AddScoped<IProductApiClient, ProductApiClient>();
            return services;
        }
    }
}