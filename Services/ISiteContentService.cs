using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webcoreapp.Models;

namespace webcoreapp.Services
{
    public interface ISiteContentService
    {
        IDictionary<string, SiteContent> GetSiteContentDictionaryByMarketplaceId(int marketPlaceId, string templateMachineName, string pageMachineName);
        IDictionary<string, SiteContent> GetSiteContentDictionary(Marketplace marketPlace, string pageMachineName);


    }
}
