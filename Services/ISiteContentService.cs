using System.Collections.Generic;
using webui.Models;

namespace webui.Services
{
    public interface ISiteContentService
    {
        IDictionary<string, SiteContent> GetSiteContentDictionaryByMarketplaceId(int marketPlaceId, string templateMachineName, string pageMachineName);
        IDictionary<string, SiteContent> GetSiteContentDictionary(Marketplace marketPlace, string pageMachineName);


    }
}
