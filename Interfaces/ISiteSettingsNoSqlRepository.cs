using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using webui.Models;

namespace webui.Interfaces
{
    public interface ISiteSettingsNoSqlRepository
    {
        Task<IDictionary<string, SiteSettings>> GetSiteSettingsByMarketplaceId(string marketPlaceId);
        Task<IList<BlockImage>> GetSiteImagesByMarketplaceId(string marketPlaceId);
        Task<IList<BlockLink>> GetSiteLinksByMarketplaceId(string marketPlaceId);
        Task<IList<SiteImage>> QueryImagesByMarketplaceId(string marketPlaceId, string containerId);
        Task<IList<SiteLink>> QueryLinksByMarketplaceId(string marketPlaceId, string containerId);
        Task<IList<SiteSettings>> QuerySiteSettingsByMarketplaceId(string marketPlaceId, string containerId);
        Task<IList<BlockImage>> QueryBlockImagesByMarketplaceId(string marketPlaceId, string containerId);
        Task<IList<BlockLink>> QueryBlockLinksByMarketplaceId(string marketPlaceId, string containerId);
    }
}
