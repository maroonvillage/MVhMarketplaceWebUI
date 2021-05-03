using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using webui.Data;
using webui.Services;

namespace webui.Services
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
