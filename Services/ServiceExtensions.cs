using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webcoreapp.Data;

namespace webcoreapp.Services
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterServices(
            this IServiceCollection services)
        {
            services.AddTransient<IMarketplaceService, MarketplaceService>();
            services.AddTransient<ISiteContentService, SiteContentService>();
            services.AddTransient<IMarketplaceRepository, MarketplaceRepository>();
            services.AddTransient<ISiteContentRepository, SiteContentRepository>();
            // Add all other services here.
            return services;
        }
    }
}
