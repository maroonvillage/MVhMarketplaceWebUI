using System;
using System.Collections.Generic;
using webui.Interfaces;

namespace webui
{
    public class DependencyResolverWrapper : IDependencyResolver
    {
        public object GetService(Type serviceType)
        {
            return null;// DependencyResolver.Current.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return null;// DependencyResolver.Current.GetServices(serviceType);
        }

        public TService GetService<TService>()
        {
            return default(TService);// ependencyResolver.Current.GetService<TService>();
        }

        public IEnumerable<TService> GetServices<TService>()
        {
            return null;// DependencyResolver.Current.GetServices<TService>();
        }
    }
}
