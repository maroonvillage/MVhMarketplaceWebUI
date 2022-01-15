using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using webui.Models;

namespace webui.Interfaces
{
    public interface IMarketplaceNoSqlRepository
    {
        Task<Marketplace> GetMarketplaceByDomain(string url);
        Task<Marketplace> GetMarketplaceById(string marketplaceId);
        Task<Menu> GetMenuByNameAsync(string marketplaceId, string menuName);
        Task<Marketplace> QueryMarketplaceByDomainAsync(string url, string containerId);
        Task<Marketplace> QueryMarketplaceByIdAsync(string marketplaceId, string containerId);
        Task<Menu> QueryMenuByNameAsync(string marketplaceId, string menuName, string containerId);

    }
}
