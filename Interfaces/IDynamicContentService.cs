using System;
using webui.Models;

namespace webui.Interfaces
{
    public interface IDynamicContentService
    {
        dynamic GetData(SiteContent content);
    }
}
