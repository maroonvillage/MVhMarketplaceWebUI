namespace webui.Models
{
    public interface IShopModel
    {
         int ShopId { get; set; }
         string ShopName { get; set; }
         string Description { get; set; }
         short Established { get; set; }
         string OwnerFirstName { get; set; }
         string OwnerLastName { get; set; }
         string Email { get; set; }
         string ShopPhone { get; set; }
         string MobilePhone { get; set; }
         string OwnerId { get; }
    }
}