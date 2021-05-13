using webui.Data;
using webui.Interfaces;
using webui.Models;

namespace webui.Services
{
    public class MarketplaceService : IMarketplaceService
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
    }
}
