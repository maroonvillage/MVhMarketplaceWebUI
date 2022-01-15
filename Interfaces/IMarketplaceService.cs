using System.Collections.Generic;
using System.Threading.Tasks;
using marketplacewebcore.Areas.Identity.Pages.Account.Manage;
using webui.Areas.Identity.Pages.Account.Manage;
using webui.Models;

namespace webui.Interfaces
{
    public interface IMarketplaceService
    {
        Marketplace GetMarketplaceByDomain(string domain);

        Marketplace GetMarketplaceById(string marketPlaceId);

        Menu GetMenuByName(string marketPlaceId, string menuName);

        void SaveShopData(ShopModel.InputModel shopModel);

        Task<decimal> SaveLocation(LocationsModel.InputModel locationModel);
        Task<ShopModel.InputModel> GetShopModelById(string id);

        Task<IList<LocationsModel.InputModel>> GetLocationsByShopId(int shopId);

        Task<IList<Amenity>> GetAmenities();

        Task<IList<HairStyle>> GetHairStyles();
        Task<IList<HairStyle>> GetHairStylesByShopId(int shopId);

        Task<IList<HairStyle>> GetSelectedHairStyles(int shopId);

        Task<MarketplaceModel> GetMarketplaceData();

    }
}
