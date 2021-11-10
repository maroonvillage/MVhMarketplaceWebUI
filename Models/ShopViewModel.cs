using System;
using System.ComponentModel.DataAnnotations;

namespace webui.Models
{

    public class ShopViewModel : IShopModel
    {

        public ShopViewModel()
        {
        }

        public ShopViewModel(int shopId, string shopName, string description, short established, string ownerId, string ownerFirstName, string ownerLastName, string email, string shopPhone, string mobilePhone)
        {
            this.ShopId = shopId;
            this.ShopName = shopName;
            this.Description = description;
            this.Established = established;
            this.OwnerId = ownerId;
            this.OwnerFirstName = ownerFirstName;
            this.OwnerLastName = ownerLastName;
            this.Email = email;
            this.ShopPhone = shopPhone;
            this.MobilePhone = mobilePhone;

        }

        public int ShopId { get; set; }
        public string ShopName { get; set; }
        public string Description { get; set; }
        public short Established { get; set; }
        public string OwnerId { get; set; }
        public string OwnerFirstName { get; set; }

        public string OwnerLastName { get; set; }

        public string Email { get; set; }
        public string ShopPhone { get; set; }

        public string MobilePhone { get; set; }


        public override string ToString()
        {
            var propStrings = $"{ShopId.ToString()} {ShopName} {Description} {Established.ToString()} {OwnerId} {OwnerFirstName} {OwnerLastName} {Email} {MobilePhone} {ShopPhone}";
            return $"{base.ToString()} {propStrings}";
        }



    }

}