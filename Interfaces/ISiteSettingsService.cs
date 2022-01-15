using System;
using System.Collections.Generic;
using webui.Models;

namespace webui.Interfaces
{
    public interface ISiteSettingsService
    {

        IDictionary<string, SiteSettings> GetSiteSettingsByMarketplaceId(string marketPlaceId);
        IList<SiteImage> GetSiteImagesByMarketplaceId(string marketPlaceId, int blockId);
        IList<SiteLink> GetSiteLinksByMarketplaceId(string marketPlaceId, int blockId);
    }
}
