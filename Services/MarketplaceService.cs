using System.Collections.Generic;
using System.Threading.Tasks;
using marketplacewebcore.Areas.Identity.Pages.Account.Manage;
using webcoreapp.Enumerators;
using webui.Areas.Identity.Pages.Account.Manage;
using webui.Enums;
using webui.Interfaces;
using webui.Models;

namespace webui.Services
{
    public class MarketplaceService : IMarketplaceService, IDynamicContentProvider
    {


        //private readonly ICacheService _memoryCache;
        private readonly IMarketplaceRepository _marketplaceRepository;
        private readonly ICacheService _cacheService;
        public MarketplaceService(IMarketplaceRepository marketPlaceRepository)
        {
            //_memoryCache = memoryCache;
            _marketplaceRepository = marketPlaceRepository;
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

        public Marketplace GetMarketplaceById(string marketPlaceId)
        {
            return new Marketplace();
        }

        public Menu GetMenuByName(string marketPlaceId, string menuName)
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


        public async void SaveShopData(ShopModel.InputModel shopModel)
        {
            var rowsAffected = 0;
            var newId = 0M;

            if (shopModel.ShopId == 0)
            {
                newId = await _marketplaceRepository.AddShop(shopModel);

                await _marketplaceRepository.AddHairPro((int)newId, shopModel.HairProfessional);

                rowsAffected = await _marketplaceRepository.SaveHairStyles((int)newId, shopModel.HairStyles);
            }
            else
            {
                rowsAffected = await _marketplaceRepository.SaveShopData(shopModel);
                await _marketplaceRepository.SaveHairPro(shopModel.ShopId, shopModel.HairProfessional);
                await _marketplaceRepository.SaveHairStyles(shopModel.ShopId, shopModel.HairStyles);
            }
            if (rowsAffected <= 0)
            {
                throw new System.Exception("No rows affected.");
            }
        }

        public async Task<ShopModel.InputModel> GetShopModelById(string id)
        {
            return await _marketplaceRepository.GetShopModelById(id);
        }

        public async Task<IList<LocationsModel.InputModel>> GetLocationsByShopId(int shopId)
        {
            return await _marketplaceRepository.GetLocationsByShopId(shopId);
        }

        public async Task<decimal> SaveLocation(LocationsModel.InputModel locationModel)
        {
            var id = 0M;
            var affectedRecords = 0;
            if (locationModel.LocationId > 0)
            {
                //call edit
            }
            else
            {
                //call add
                id = await _marketplaceRepository.AddLocation(locationModel);
                locationModel.LocationId = (int)id;
                affectedRecords = await _marketplaceRepository.SaveAmenities(locationModel.ShopId, locationModel.LocationId, locationModel.Amenities);
            }

            return id;
        }

        public async Task<IList<Amenity>> GetAmenities()
        {
            //var cacheKey = HelpersService.GetEnumDescription(CacheKeys.Amenities);
            //var amenities = await _memoryCache.Get(cacheKey);
            //if (amenities == null)
            //{
             var   amenities = await _marketplaceRepository.GetAmenities();
                //_memoryCache.Set<IList<Amenity>>(cacheKey, amenities);
            //}//

            return amenities;
        }

        public async Task<IList<HairStyle>> GetHairStyles()
        {
            //var cacheKey = HelpersService.GetEnumDescription(CacheKeys.HairStyles);

            //var hairStyles = await _memoryCache.GetAsync<IList<HairStyle>>(cacheKey);
            //if (hairStyles == null)
            //{
               var hairStyles = await _marketplaceRepository.GetHairStyles();
               // _memoryCache.Set<IList<HairStyle>>(cacheKey, hairStyles);
            //}

            return hairStyles;
        }


        public async Task<IList<HairStyle>> GetHairStylesByShopId(int shopId)
        {

            return await _marketplaceRepository.GetHairStylesByShopId(shopId);

        }

        public async Task<IList<HairStyle>> GetSelectedHairStyles(int shopId)
        {

            var allStyles = (List<HairStyle>)await _marketplaceRepository.GetHairStyles();

            var stylesByShopId = (List<HairStyle>)await _marketplaceRepository.GetHairStylesByShopId(shopId);

            allStyles.ForEach(delegate (HairStyle style)
            {
                var selected = stylesByShopId.Find(s => s.StyleId == style.StyleId);
                style.IsSelected = selected != null ? true : false;

            });

            return allStyles;
        }

        public async Task<MarketplaceModel> GetMarketplaceData()
        {
            //var cacheKey = HelpersService.GetEnumDescription(CacheKeys.HairPros);

           // var hairPros = await _memoryCache.GetAsync<IEnumerable<HairPro>>(cacheKey);
            //if (hairPros == null)
            //{
              var  hairPros = (IEnumerable<HairPro>)_marketplaceRepository.GetAllHairPros();
               // _memoryCache.Set<IEnumerable<HairPro>>(cacheKey, hairPros);
            //}

           // cacheKey = HelpersService.GetEnumDescription(CacheKeys.MarketplaceShops);
            //var shops = await _memoryCache.GetAsync<IEnumerable<IShopModel>>(cacheKey);
            //if (shops == null)
            //{
            var    shops = (IEnumerable<IShopModel>)_marketplaceRepository.GetAllMarketplaceShops();
                //_memoryCache.Set<IEnumerable<IShopModel>>(cacheKey, shops);
            //}

            //cacheKey = HelpersService.GetEnumDescription(CacheKeys.Locations);

            //var locations = await _memoryCache.GetAsync<IEnumerable<ILocationsModel>>(cacheKey);
            //if (locations == null)
            //{
               var locations = (IEnumerable<ILocationsModel>)_marketplaceRepository.GetAllLocations();
                //_memoryCache.Set<IEnumerable<ILocationsModel>>(cacheKey, locations);
            //}

            //cacheKey = HelpersService.GetEnumDescription(CacheKeys.ShopAmenities);

            //var amenities = await _memoryCache.GetAsync<IEnumerable<ShopAmenityModel>>(cacheKey);
            //if (amenities == null)
            //{
              var  amenities = (IEnumerable<ShopAmenityModel>)_marketplaceRepository.GetAllShopAmenities();
               /// _memoryCache.Set<IEnumerable<ShopAmenityModel>>(cacheKey, amenities);
            //}

            //cacheKey = HelpersService.GetEnumDescription(CacheKeys.ShopHairStyles);

            //var styles = await _memoryCache.GetAsync<IEnumerable<ShopHairStyleModel>>(cacheKey);
            //if (styles == null)
            //{
             var   styles = (IEnumerable<ShopHairStyleModel>)_marketplaceRepository.GetAllShopHairStyles();
                //_memoryCache.Set<IEnumerable<ShopHairStyleModel>>(cacheKey, shops);
            //}

            var marketplaceModel = new MarketplaceModel
            {
                HairPros = hairPros,
                Shops = shops,
                Locations = locations,
                ShopHairStyles = styles,
                ShopAmenities = amenities
            };

            return await Task.FromResult<MarketplaceModel>(marketplaceModel);

        }



    }
}
