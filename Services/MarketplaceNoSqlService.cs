using System;
using System.Threading.Tasks;
using webcoreapp.Enumerators;
using webui.Interfaces;
using webui.Models;

namespace webui.Services
{
    public class MarketplaceNoSqlService : IMarketplaceNoSqlService, IDynamicContentProvider
    {

        private readonly IMarketplaceNoSqlRepository _marketplaceNoSqlRepository;
        private readonly ICacheService _cacheService;
        public MarketplaceNoSqlService(IMarketplaceNoSqlRepository marketPlaceNoSqlRepository)
        {
            //_memoryCache = memoryCache;
            _marketplaceNoSqlRepository = marketPlaceNoSqlRepository;
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

        public Menu GetMenuByName(string marketPlaceId, string menuName)
        {
            return  _marketplaceNoSqlRepository.GetMenuByNameAsync(marketPlaceId, menuName).Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public async Task<Marketplace> GetMarketplaceByDomainAsync(string domain)
        {
            var mrktPlc = await _marketplaceNoSqlRepository.GetMarketplaceByDomain(domain);

            return mrktPlc ?? new Marketplace();
        }

        public async Task<Marketplace> GetMarketplaceByIdAsync(string marketplaceId)
        {
            var mrktPlc = await _marketplaceNoSqlRepository.GetMarketplaceById(marketplaceId);

            return mrktPlc ?? new Marketplace();
        }


    }
}
