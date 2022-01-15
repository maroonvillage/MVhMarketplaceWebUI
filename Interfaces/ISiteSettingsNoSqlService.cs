using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using webui.Models;

namespace webui.Interfaces
{
    public interface ISiteSettingsNoSqlService
    {
        Task<IDictionary<string, SiteSettings>> GetSiteSettingsByMarketplaceId(string marketPlaceId);
        IList<BlockImage> GetSiteImagesByMarketplaceId(string marketPlaceId, int blockId);
        IList<BlockLink> GetSiteLinksByMarketplaceId(string marketPlaceId, int blockId);
    }
}
