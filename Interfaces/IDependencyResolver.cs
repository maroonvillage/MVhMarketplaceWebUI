using System;
using System.Collections.Generic;

namespace webui.Interfaces
{
    public interface IDependencyResolver
    {
        object GetService(Type serviceType);

        IEnumerable<object> GetServices(Type serviceType);

        TService GetService<TService>();

        IEnumerable<TService> GetServices<TService>();
    }
}
