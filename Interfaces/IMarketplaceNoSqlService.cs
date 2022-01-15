using System;
using System.Threading.Tasks;
using webui.Models;

namespace webui.Interfaces
{
    public interface IMarketplaceNoSqlService
    {

        Task<Marketplace> GetMarketplaceByDomainAsync(string domain);
        Task<Marketplace> GetMarketplaceByIdAsync(string marketplaceId);
    }
}
