using System;
namespace webui.Models
{
    public class LocationViewModel : ILocationsModel
    {

        public int LocationId { get; set; }
        public int ShopId { get; set; }

        public string LocationName { get; set; }
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }
    }

}
