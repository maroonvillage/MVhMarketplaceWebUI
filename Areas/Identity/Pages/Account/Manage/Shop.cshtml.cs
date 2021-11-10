using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using marketplacewebcore.Areas.Identity.Pages.Account.Manage;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using webui.Interfaces;
using webui.Models;

namespace webui.Areas.Identity.Pages.Account.Manage
{
    public class ShopModel : PageModel, IShopModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<PersonalDataModel> _logger;

        private readonly IMarketplaceService _marketPlaceService;

        public ShopModel(
            UserManager<IdentityUser> userManager,
            IMarketplaceService marketPlaceService,
            ILogger<PersonalDataModel> logger)
        {
            _userManager = userManager;
            _marketPlaceService = marketPlaceService;
            _logger = logger;

        }

        public int ShopId { get; set; }
        [Display(Name = "Marketplace Name")]
        public string ShopName { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Established")]

        public short Established { get; set; }
        [Display(Name = "First Name")]

        public string OwnerFirstName { get; set; }
        [Display(Name = "Last Name")]
        public string OwnerLastName { get; set; }
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        [Display(Name = "Shop Phone")]
        public string ShopPhone { get; set; }
        [Display(Name = "Mobile Phone")]
        public string MobilePhone { get; set; }
        [Display(Name = "I am a shop owner: ")]
        [BindProperty]
        public bool IsShopOwner { get; set; }
        public string OwnerId
        {
            get
            {
                var user = _userManager.GetUserAsync(User);
                return user != null ? user.Result.Id : string.Empty;
            }

        }
        [BindProperty]
        public IList<SelectListItem> HairStyles { get; set; }

        public HairPro HairProfessional { get; set; }


        public override string ToString()
        {
            var propStrings = $"{ShopId.ToString()} {ShopName} {Description} {Established.ToString()} {OwnerId} {OwnerFirstName} {OwnerLastName} {Email} {ShopPhone} {MobilePhone}";
            var styles = (List<SelectListItem>)HairStyles;
            var selectedItemsStr = string.Empty;

            styles.ForEach(delegate (SelectListItem slItem)
            {
                selectedItemsStr += $"{slItem.Value.ToString()} {slItem.Text}";
            });

            return $"{propStrings} {selectedItemsStr}";
        }

        [TempData]
        public string StatusMessage { get; set; }


        public IList<SelectListItem> Options { get; set; }

        public class InputModel
        {
            public InputModel() { }
            public InputModel(int shopId, string shopName, string description, short established, string ownerFirstName,
                            string ownerLastName, string email, string shopPhone, string mobilePhone)
            {
                this.ShopId = shopId;
                this.ShopName = shopName;
                this.Description = description;
                this.Established = established;
                this.OwnerFirstName = ownerFirstName;
                this.OwnerLastName = ownerLastName;
                this.Email = email;
                this.ShopPhone = ShopPhone;
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

            public IList<HairStyle> HairStyles { get; set; }

            public HairPro HairProfessional { get; set; }

            public override string ToString()
            {
                var propStrings = $"{ShopId.ToString()} {ShopName} {Description} {Established.ToString()} {OwnerId} {OwnerFirstName} {OwnerLastName} {Email} {ShopPhone} {MobilePhone}";
                var styles = (List<HairStyle>)HairStyles;
                var stylesStr = string.Empty;

                styles.ForEach(delegate (HairStyle hairStyle)
                {
                    stylesStr += $"{hairStyle.StyleId.ToString()} {hairStyle.StyleName}";
                });

                return $"{propStrings} {stylesStr}";
            }

        }//end InputModel class
        [BindProperty]
        public InputModel Input { get; set; }

        private async Task LoadAsync(IdentityUser user)
        {
            var shop = await _marketPlaceService.GetShopModelById(user.Id);


            ShopId = shop.ShopId;
            ShopName = shop.ShopName;
            Description = shop.Description;
            OwnerFirstName = shop.OwnerFirstName;
            OwnerLastName = shop.OwnerLastName;
            Email = shop.Email;
            ShopPhone = shop.ShopPhone;
            MobilePhone = shop.MobilePhone;
            IsShopOwner = shop.HairProfessional.IsShopOwner;

            var hairStyles = (List<HairStyle>)await _marketPlaceService.GetSelectedHairStyles(shop.ShopId);

            var list = new List<SelectListItem>();
            hairStyles.ForEach(delegate (HairStyle style)
            {
                list.Add(
                new SelectListItem
                {
                    Value = style.StyleId.ToString(),
                    Text = style.StyleName,
                    Selected = style.IsSelected
                });
            });
            HairStyles = list;

        }//end LoadAsync

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);

            return Page();
        }//end OnGet

        public async Task<IActionResult> OnPostSaveShopAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var userId = await _userManager.GetUserIdAsync(user);

            var shop = await _marketPlaceService.GetShopModelById(userId);

            Input.ShopId = shop.ShopId;
            Input.OwnerId = userId;

            // var hashedInput = CheckShopDataForChanges(Input.ToString());
            // var hashedSaved = CheckShopDataForChanges(shop.ToString());

            // if (hashedInput != hashedSaved)
            // {
            var selectItems = (List<SelectListItem>)HairStyles;
            var styles = new List<HairStyle>();

            selectItems.ForEach(delegate (SelectListItem slItem)
            {
                if (slItem.Selected)
                {
                    styles.Add(
                        new HairStyle
                        {
                            StyleId = Convert.ToInt32(slItem.Value),
                            StyleName = slItem.Text
                        }
                    );
                }
            });
            Input.HairStyles = styles;

            Input.HairProfessional = new HairPro{

                HairProName = $"{Input.OwnerFirstName} {Input.OwnerLastName}",
                IsShopOwner = IsShopOwner,
                YearsOfExperience = 1

            };

            _marketPlaceService.SaveShopData(Input);
            //}
            StatusMessage = "Your shop info has been updated";
            await LoadAsync(user);
            return Page();
            //return RedirectToPage();
        }//end OnPostSave...

        private string CheckShopDataForChanges(string input)
        {
            var hashed = string.Empty;
            using (var algorithm = SHA512.Create()) //or MD5 SHA256 etc.
            {
                var hashedBytes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

                hashed = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }

            return hashed;
        }
    }// ShopModel class


}