using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using webui.Data;
using webui.Interfaces;
using webui.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterServices(
            this IServiceCollection services)
        {
            services.AddScoped<IEmailService, EmailService>();

            services.AddTransient<IMarketplaceService, MarketplaceService>();
            services.AddTransient<ISiteContentService, SiteContentService>();
            services.AddTransient<ISiteSettingsService, SiteSettingsService>();
            services.AddTransient<ISiteSettingsRepository, SiteSettingsRepository>();

            services.AddTransient<IMarketplaceRepository, MarketplaceRepository>();
            services.AddTransient<ISiteContentRepository, SiteContentRepository>();

            services.AddTransient<IDynamicContentProvider, MarketplaceService>();
            services.AddTransient<IDynamicContentProvider, SiteSettingsService>();
            //services.AddTransient<ICacheService, CacheService<T>(IMemoryCache cache);


            // Add all other services here.
            _ = services.AddSingleton<IDynamicContentService, DynamicContentService>((dcx) =>
              {
                  IServiceProvider sp = dcx.GetService<IServiceProvider>();
                  IEnumerable<IDynamicContentProvider> providers = sp.GetServices<IDynamicContentProvider>();

                  return new DynamicContentService(providers, sp);

              });

            _ = services.AddSingleton<IContentHelperService, ContentHelperService>((ctx) =>
              {
                //IServiceProvider sp = ctx.GetRequiredService<IServiceProvider>();
                IServiceProvider sp = ctx.GetService<IServiceProvider>();

                  return new ContentHelperService(sp);
              });


            return services;
        }

        public static IServiceCollection GetServices(this IServiceCollection services)
        {
            return services.GetServices();
        }

        public static ServiceProvider GetServiceProvider(this IServiceCollection services)
        {
            return services.BuildServiceProvider();
        }
       
    }
}
