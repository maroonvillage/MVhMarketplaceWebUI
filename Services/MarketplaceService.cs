using webcoreapp.Enumerators;
using webui.Interfaces;
using webui.Models;

namespace webui.Services
{
    public class MarketplaceService : IMarketplaceService, IDynamicContentProvider
    { 
        private readonly IMarketplaceRepository _marketplaceRepository;
        public MarketplaceService(IMarketplaceRepository marketplaceRepository)
        {
            _marketplaceRepository = marketplaceRepository;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public Marketplace GetMarketplaceByDomain(string domain)
        {
            var mrktPlc = _marketplaceRepository.GetMarketplaceByDomain(domain);

            return mrktPlc ?? new Marketplace();
        }

        public Marketplace GetMarketplaceById(int marketPlaceId)
        {
            return new Marketplace();
        }

        public Menu GetMenuByName(int marketPlaceId, string menuName)
        {
            return _marketplaceRepository.GetMenuByName(marketPlaceId, menuName);
        }


        public bool CanProviderData(SiteContent siteContent)
        {
            switch (siteContent.ContentType)
            {
                case DynamicContentType.Menu:
                    return true;
                default:
                    return false;
            }
        }


        public dynamic GetData(SiteContent siteContent)
        {
            switch (siteContent.ContentType)
            {
                case DynamicContentType.Menu:
                    return GetMenuByName(siteContent.MarketplaceId, siteContent.ContentName);
                default:
                    return false;
            }
        }

    }
}
