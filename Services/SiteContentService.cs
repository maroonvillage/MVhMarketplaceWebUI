using System;
using System.Collections.Generic;
using System.Linq;
using webui.Interfaces;
using webui.Models;

namespace webui.Services
{
    public class SiteContentService : ISiteContentService
    {

        private readonly ISiteContentRepository _siteContentRepository;
        public SiteContentService()
        {

        }

        public SiteContentService(ISiteContentRepository siteContentRepository)
        {
            _siteContentRepository = siteContentRepository;
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

            return GetSiteContentDictionaryByMarketplaceId(marketPlace.MarketplaceId, marketPlace.Settings.Tempate.TemplateMachineName, pageMachineName);
        }


        public IDictionary<string, SiteContent> GetSiteContentDictionaryByMarketplaceId(int marketPlaceId, string templateMachineName, string pageMachineName)
        {
            if (string.IsNullOrWhiteSpace(templateMachineName))
            {
                throw new ArgumentNullException("templateMachineName");
            }

            if (string.IsNullOrWhiteSpace(pageMachineName))
            {
                throw new ArgumentNullException("pageMachineName");
            }

            //var cacheKey = string.Format(HelpersService.GetEnumDescription(CacheKeys.SiteContentDictionary), dealerId, templateMachineName, pageMachineName);

            //if (_cacheManager.Contains(cacheKey))
            //{
            //    return (IDictionary<string, SiteContent>)_cacheManager[cacheKey] ?? new Dictionary<string, SiteContent>();
            //}

            var siteContentDictionary = _siteContentRepository.GetSiteContentDictionaryByMarketplaceId(marketPlaceId, templateMachineName, pageMachineName).ToDictionary(x => x.Block.BlockMachineName);
            if (siteContentDictionary != null)
            {
                //TODO: Add dictionary to cache.

                //_absoluteCacheExpiryTime = HelpersService.GetAbsoluteCacheExpiry(HelpersService.GetEnumDefaultValue(CacheKeys.SiteContentDictionary), TimeIncrements.Minutes);
                //_cacheManager.Add(cacheKey, siteContentDictionary, CacheItemPriority.Normal, null, _absoluteCacheExpiryTime);
            }

            return siteContentDictionary ?? new Dictionary<string, SiteContent>();
        }
    }
}
