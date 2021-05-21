using System;
using System.Collections.Generic;
using System.Linq;
using webcoreapp.Enumerators;
using webui.Interfaces;
using webui.Models;

namespace webui.Services
{
    public class DynamicContentService : IDynamicContentService
    {
        private readonly IEnumerable<IDynamicContentProvider> providers;

        //private readonly IDependencyResolver dependencyResolver;

        private readonly IServiceProvider _serviceProvider;

        public DynamicContentService(IEnumerable<IDynamicContentProvider> providers, IServiceProvider serviceProvider)
        {
            this.providers = providers;
            //this.dependencyResolver = dependencyResolver;
            _serviceProvider = serviceProvider;
        }

        public dynamic GetData(SiteContent content)
        {
            return ProviderFactory(content).GetData(content);
        }

        private IDynamicContentProvider ProviderFactory(SiteContent content)
        {
            return (providers.FirstOrDefault(service => service.CanProviderData(content)) ?? (IDynamicContentProvider)_serviceProvider.GetService(typeof(IDynamicContentProvider)));
        }
    }

    public class NullDynamicContentProvider : IDynamicContentProvider
    {
        bool IDynamicContentProvider.CanProviderData(SiteContent siteContent)
        {
            return siteContent.ContentType == DynamicContentType.Unknown || siteContent.ContentType == DynamicContentType.None;
        }

        dynamic IDynamicContentProvider.GetData(SiteContent content)
        {
            return null;
        }
    }
}
