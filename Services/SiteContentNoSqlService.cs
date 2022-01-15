using System;
using System.Collections.Generic;
using System.Linq;
using webui.Interfaces;
using webui.Models;

namespace webui.Services
{
    public class SiteContentNoSqlService : ISiteContentNoSqlService
    {
        private readonly ISiteContentNoSqlRepository _siteContentNoSqlRepository;
        public SiteContentNoSqlService()
        {

        }

        public SiteContentNoSqlService(ISiteContentNoSqlRepository siteContentNoSqlRepository)
        {
            _siteContentNoSqlRepository = siteContentNoSqlRepository;
        }


        public IDictionary<string, SiteContent> GetSiteContentDictionaryByMarketplaceId(string marketPlaceId, string templateMachineName, string pageMachineName)
        {
            if (string.IsNullOrWhiteSpace(templateMachineName))
            {
                throw new ArgumentNullException("templateMachineName");
            }

            if (string.IsNullOrWhiteSpace(pageMachineName))
            {
                throw new ArgumentNullException("pageMachineName");
            }

            var siteContentDictionary = _siteContentNoSqlRepository.GetSiteContentDictionaryByMarketplaceId(marketPlaceId, templateMachineName, pageMachineName).ToDictionary(x => x.Block.BlockMachineName);
            
            if (siteContentDictionary != null)
            {
                //TODO: Add dictionary to cache.
            }

            return siteContentDictionary ?? new Dictionary<string, SiteContent>();

        }
        public IDictionary<string, SiteContent> GetSiteContentDictionary(Marketplace marketPlace, string pageMachineName)
        {
            if (marketPlace == null)
            {
                throw new ArgumentNullException("marketplace");
            }

            if (string.IsNullOrWhiteSpace(pageMachineName))
            {
                throw new ArgumentNullException("pageMachineName");
            }

            if (marketPlace.Settings == null)
            {
                throw new NullReferenceException("marketPlace.Settings is null");
            }

            return GetSiteContentDictionaryByMarketplaceId(marketPlace.MarketplaceId, marketPlace.Settings.Template.TemplateMachineName, pageMachineName);

        }
    }
}
