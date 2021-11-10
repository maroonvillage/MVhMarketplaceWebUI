using System.Collections.Generic;
using System.Threading.Tasks;
using marketplacewebcore.Areas.Identity.Pages.Account.Manage;
using webui.Areas.Identity.Pages.Account.Manage;
using webui.Models;

namespace webui.Interfaces
{
    public interface IMarketplaceRepository
    {
        Marketplace GetMarketplaceByDomain(string domain);
        Marketplace GetMarketplaceById(int marketPlacdId);
        MarketplaceSetting GetMarketplaceSettingsById(int marketPlaceId);
        MarketplaceTheme GetThemeByMarketplaceId(int marketPlaceId);
        Template GetTemplateById(int templateId);
        Menu GetMenuByName(int marketPlaceId, string menuName);


        Task<decimal> AddShop(ShopModel.InputModel shopModel);
        Task<decimal> AddLocation(LocationsModel.InputModel locationModel);
        Task<int> EditLocation(LocationsModel.InputModel locationModel);
        Task<int> SaveShopData(ShopModel.InputModel shopModel);
        Task<int> SaveAmenities(int shopId, int locationId, IList<Amenity> amenities);
        Task<int> DeleteAmenity(int shopId, int locationId, int amenity = 0);
        Task<ShopModel.InputModel> GetShopModelById(string id);
        Task<IList<LocationsModel.InputModel>> GetLocationsByShopId(int shopId);
        Task<IList<Amenity>> GetAmenities();
        Task<IList<HairStyle>> GetHairStyles();

        Task<int> DeleteHairStyle(int shopId, int id = 0, int hairStyleId = 0);
        Task<int> SaveHairStyles(int shopId, IList<HairStyle> styles);
        Task<IList<HairStyle>> GetHairStylesByShopId(int shopId);

        Task<decimal> AddHairPro(int shopId, HairPro hairPro);

        Task<int> SaveHairPro(int shopId, HairPro hairPro);

        IEnumerable<IShopModel> GetAllMarketplaceShops();

        IEnumerable<HairPro> GetAllHairPros();
        IEnumerable<ILocationsModel> GetAllLocations();

        IEnumerable<ShopAmenityModel> GetAllShopAmenities();

        IEnumerable<ShopHairStyleModel> GetAllShopHairStyles();

    }
}
