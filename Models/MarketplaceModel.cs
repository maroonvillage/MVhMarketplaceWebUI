using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace webui.Models
{
    public class MarketplaceModel
    {
        public IEnumerable<ILocationsModel> Locations { get; set; }
        public IEnumerable<IShopModel> Shops { get; set; }

        public IEnumerable<HairPro> HairPros { get; set; }

        public IEnumerable<ShopAmenityModel> ShopAmenities { get; set; }

        public IEnumerable<ShopHairStyleModel> ShopHairStyles { get; set; }

        public IList<SelectListItem> HairStyles { get; set; }

        public IList<SelectListItem> Amenities { get; set; }

        public string AmenitiesSelected { get; set; }

        public string StylesSelected { get; set; }

        
    } 
}