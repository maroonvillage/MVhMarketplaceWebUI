using System;
using webui.Models;

namespace webui.Interfaces
{
    public interface IDynamicContentProvider
    {
        bool CanProviderData(SiteContent siteContent);
        dynamic GetData(SiteContent siteContent);
    }
}
