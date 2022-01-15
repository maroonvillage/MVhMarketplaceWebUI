using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webcoreapp.Enumerators;
using webui.Interfaces;
using webui.Models;

namespace webui.Services
{
    public class SiteSettingsNoSqlService : ISiteSettingsNoSqlService, IDynamicContentProvider
    {
        private readonly ISiteSettingsNoSqlRepository _siteSettingseNoSqlRepository;

        public SiteSettingsNoSqlService(ISiteSettingsNoSqlRepository siteSettingseNoSqlRepository)
        {
            _siteSettingseNoSqlRepository = siteSettingseNoSqlRepository;
        }


        public async Task<IDictionary<string, SiteSettings>> GetSiteSettingsByMarketplaceId(string marketPlaceId)
        {
            return await _siteSettingseNoSqlRepository.GetSiteSettingsByMarketplaceId(marketPlaceId);
        }
        public IList<BlockImage> GetSiteImagesByMarketplaceId(string marketPlaceId, int blockId)
        {

            //TODO: check cache first
            var siteImages =  _siteSettingseNoSqlRepository.GetSiteImagesByMarketplaceId(marketPlaceId).Result;

            //Use blockId to filter results
            //LINQ Method Syntax.
            var results = siteImages.Where(i => i.BlockId == blockId);


            return results.ToList<BlockImage>();
        }
        public IList<BlockLink> GetSiteLinksByMarketplaceId(string marketPlaceId, int blockId)
        {
            //TODO: check cache first
            var siteLinks = _siteSettingseNoSqlRepository.GetSiteLinksByMarketplaceId(marketPlaceId).Result;

            //Use blockId to filter results
            //LINQ Method Syntax.
            var results = siteLinks.Where(i => i.BlockId == blockId);


            return results.ToList<BlockLink>();
        }

        public bool CanProviderData(SiteContent siteContent)
        {
            switch (siteContent.ContentType)
            {
                case DynamicContentType.SiteSettings:
                    return true;
                default:
                    return false;
            }
        }

        public dynamic GetData(SiteContent siteContent)
        {
            switch (siteContent.ContentType)
            {
                case DynamicContentType.SiteSettings:
                    return GetSiteSettingsByMarketplaceId(siteContent.MarketplaceId);
                case DynamicContentType.SiteImage:
                    return GetSiteImagesByMarketplaceId(siteContent.MarketplaceId, siteContent.Block.BlockId);
                case DynamicContentType.Link:
                    return GetSiteLinksByMarketplaceId(siteContent.MarketplaceId, siteContent.Block.BlockId);
                default:
                    return false;
            }
        }


    }
}
