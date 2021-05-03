using webui.Models;

namespace webui.Services
{
    public interface IMarketplaceService
    {
        Marketplace GetMarketplaceByDomain(string domain);

        Marketplace GetMarketplaceById(int marketPlaceId);


    }
}
