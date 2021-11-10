using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using webui.Interfaces;
using webui.Models;

namespace marketplacewebcore.Areas.Identity.Pages.Account.Manage
{
    public class LocationsModel : PageModel, ILocationsModel
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<PersonalDataModel> _logger;

        private readonly IMarketplaceService _marketPlaceService;

        public LocationsModel(
            UserManager<IdentityUser> userManager,
            IMarketplaceService marketPlaceService,
            ILogger<PersonalDataModel> logger)
        {
            _userManager = userManager;
            _marketPlaceService = marketPlaceService;
            _logger = logger;
        }


        public int LocationId { get; set; }
        public int ShopId { get; set; }
        [Display(Name = "Location Name")]
        public string LocationName { get; set; }

        [Display(Name = "Address 1")]
        public string Address1 { get; set; }
        [Display(Name = "Address 2")]
        public string Address2 { get; set; }
        [Display(Name = "City")]

        public string City { get; set; }
        [Display(Name = "State")]
        public string State { get; set; }
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        public override string ToString()
        {
            var propStrings = $"{ShopId.ToString()} {LocationId} {ShopId} {LocationName} {Address1} {Address2} {City} {State} {ZipCode}";
            return $"{base.ToString()} {propStrings}";

        }


        [TempData]
        public string StatusMessage { get; set; }

        public IList<SelectListItem> Options { get; set; }
        [BindProperty]
        public IList<SelectListItem> Amenities { get; set; }

        public class InputModel
        {
            public InputModel() { }
            public InputModel(int shopId, int locationId, string locationName, string address1, string address2,
                            string city, string state, string zipCode)
            {
                this.ShopId = shopId;
                this.LocationId = locationId;
                this.LocationName = locationName;
                this.Address1 = address1;
                this.Address2 = address2;
                this.City = city;
                this.State = state;
                this.ZipCode = zipCode;

            }
            public int LocationId { get; set; }
            public int ShopId { get; set; }
            public string LocationName { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string City { get; set; }

            public string State { get; set; }

            public string ZipCode { get; set; }

            public IList<Amenity> Amenities { get; set; }

            public override string ToString()
            {
                var propStrings = $"{ShopId.ToString()} {LocationId} {ShopId} {LocationName} {Address1} {Address2} {City} {State.ToString()} {ZipCode}";
                return $"{base.ToString()} {propStrings}";
            }

        }//end InputModel class

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<InputModel> Locations { get; set; }

        private async Task LoadAsync(IdentityUser user)
        {
            var shop = await _marketPlaceService.GetShopModelById(user.Id);
            if (shop != null)
            {
                ShopId = shop.ShopId;
                Locations = await _marketPlaceService.GetLocationsByShopId(shop.ShopId);
            }

            IList<SelectListItem> states = new List<SelectListItem>();
            for (var i = 0; i <= 1; i++)
            {
                states.Add(
                new SelectListItem
                {
                    Value = "CA",
                    Text = "CA"
                });

            }
            Options = states;

            var amenities = (List<Amenity>)await _marketPlaceService.GetAmenities();

            var list = new List<SelectListItem>();
            amenities.ForEach(delegate (Amenity amenity)
            {
                list.Add(
                new SelectListItem
                {
                    Value = amenity.AmenityId.ToString(),
                    Text = amenity.AmenityName
                });
            });

            Amenities = list;

        }
        public async Task<IActionResult> OnGet()
        {

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostSaveLocationAsync()
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

            var shop = await _marketPlaceService.GetShopModelById(user.Id);

            var selectItems = (List<SelectListItem>)Amenities;
            var amenities = new List<Amenity>();


            selectItems.ForEach(delegate (SelectListItem slItem)
            {
                if (slItem.Selected)
                {
                    amenities.Add(
                        new Amenity
                        {
                            AmenityId = Convert.ToInt32(slItem.Value),
                            AmenityName = slItem.Text
                        }
                    );
                }
            });

            if (shop != null)
            {
                Input.ShopId = shop.ShopId;
                Input.Amenities = amenities;
            }

            var id = await _marketPlaceService.SaveLocation(Input);

            if (id <= 0)
                StatusMessage = "Location was not saved!";
            else
                StatusMessage = "Location successfully saved!";



            return RedirectToPage();
        }


    }
}