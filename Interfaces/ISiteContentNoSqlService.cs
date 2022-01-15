using System.Collections.Generic;
using webui.Models;

namespace webui.Interfaces
{
    public interface ISiteContentNoSqlService
    {
        IDictionary<string, SiteContent> GetSiteContentDictionaryByMarketplaceId(string marketPlaceId, string templateMachineName, string pageMachineName);
        IDictionary<string, SiteContent> GetSiteContentDictionary(Marketplace marketPlace, string pageMachineName);
    }
}
