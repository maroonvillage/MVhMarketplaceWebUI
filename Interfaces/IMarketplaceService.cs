using webui.Models;

namespace webui.Interfaces
{
    public interface IMarketplaceService
    {
        Marketplace GetMarketplaceByDomain(string domain);

        Marketplace GetMarketplaceById(int marketPlaceId);


    }
}
