using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using webui.Models;

namespace webui.Interfaces
{
    public interface ISiteContentNoSqlRepository
    {
        IEnumerable<SiteContent> GetSiteContentDictionaryByMarketplaceId(string marketPlaceId, string templateMachineName, string pageMachineName);
        Task<SitePage> QuerySitePageByPageMachineNameAsync(string marketplaceId, string sitePageMachineName, string containerId);
        Task<List<SiteContent>> QuerySiteContentByBlockIdsAsync(string pageMachineName, string containerId);

    }
}
