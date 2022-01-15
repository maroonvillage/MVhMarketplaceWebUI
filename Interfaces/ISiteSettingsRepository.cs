using System;
using System.Collections.Generic;
using webui.Models;

namespace webui.Interfaces
{
    public interface ISiteSettingsRepository
    {

        IDictionary<string, SiteSettings> GetSiteSettingsByMarketplaceId(string marketPlaceId);
        IList<SiteImage> GetSiteImagesByMarketplaceId(string marketPlaceId);
        IList<SiteLink> GetSiteLinksByMarketplaceId(string marketPlaceId);
    }
}
