using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webcoreapp.Data;
using webcoreapp.Models;

namespace webcoreapp.Services
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
