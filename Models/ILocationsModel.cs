namespace webui.Models
{
    public interface ILocationsModel
    {
         int LocationId { get; set; }
         int ShopId { get; set; }

         string LocationName { get; set; }
         string Address1 { get; set; }

         string Address2 { get; set; }

         string City { get; set; }

         string State { get; set; }

         string ZipCode { get; set; }

         
    }

}