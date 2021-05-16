using System.Collections.Generic;
using webui.Models;

namespace webui.Interfaces
{
    public interface ISiteContentRepository
    {
        IEnumerable<SiteContent> GetSiteContentDictionaryByMarketplaceId(int marketPlaceId, string templateMachineName, string pageMachineName);
        IDictionary<string, SiteContent> GetSiteContentDictionary(Marketplace marketPlace, string pageMachineName);
    }
}
