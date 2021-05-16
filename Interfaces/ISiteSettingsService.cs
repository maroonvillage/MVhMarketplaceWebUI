using System;
using System.Collections.Generic;
using webui.Models;

namespace webui.Interfaces
{
    public interface ISiteSettingsService
    {

        IDictionary<string, SiteSettings> GetSiteSettingsByMarketplaceId(int marketPlaceId);
        IList<SiteImage> GetSiteImagesByMarketplaceId(int marketPlaceId);
    }
}
