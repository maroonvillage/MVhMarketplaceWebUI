using System.Collections.Generic;
using System.Linq;
using webcoreapp.Enumerators;
using webui.Interfaces;
using webui.Models;

namespace webui.Services
{
    public class SiteSettingsService : ISiteSettingsService, IDynamicContentProvider
    {

        private readonly ISiteSettingsRepository _siteSettingseRepository;
        public SiteSettingsService(ISiteSettingsRepository siteSettingseRepository)
        {
            _siteSettingseRepository = siteSettingseRepository;
        }



        public IDictionary<string, SiteSettings> GetSiteSettingsByMarketplaceId(int marketPlaceId)
        {
            return _siteSettingseRepository.GetSiteSettingsByMarketplaceId(marketPlaceId);
        }

        public IList<SiteImage> GetSiteImagesByMarketplaceId(int marketPlaceId, int blockId)
        {

            //TODO: check cache first
            var siteImages =  _siteSettingseRepository.GetSiteImagesByMarketplaceId(marketPlaceId);

            //Use blockId to filter results
            //LINQ Method Syntax.
            var results = siteImages.Where(i => i.BlockImage.BlockId == blockId);

            
            return results.ToList<SiteImage>();
        }

        public IList<SiteLink> GetSiteLinksByMarketplaceId(int marketPlaceId, int blockId)
        {
            //TODO: check cache first
            var siteLinks = _siteSettingseRepository.GetSiteLinksByMarketplaceId(marketPlaceId);

            //Use blockId to filter results
            //LINQ Method Syntax.
            var results = siteLinks.Where(i => i.BlockLink.BlockId == blockId);


            return results.ToList<SiteLink>();
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
