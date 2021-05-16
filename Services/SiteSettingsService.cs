using System;
using System.Collections.Generic;
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


        public IList<SiteImage>  GetSiteImagesByMarketplaceId(int marketPlaceId)
        {
            return _siteSettingseRepository.GetSiteImagesByMarketplaceId(marketPlaceId);
        }

        public dynamic GetData(SiteContent siteContent)
        {
            switch (siteContent.ContentType)
            {
                case DynamicContentType.SiteSettings:
                    return null; // call method to get site settings
                default:
                    return false;
            }
        }

    }
}
