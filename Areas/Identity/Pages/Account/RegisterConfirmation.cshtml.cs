using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using webui.Enums;
using webui.Interfaces;
using webui.Models;

namespace webui.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : RazorBasePageModel, IPageModel
    {
        private dynamic _data;
        private SitePageType _page = SitePageType.Unknown;
        private IDictionary<string, SiteContent> _siteContentBlock;

        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _sender;
        private IMarketplaceNoSqlService _marketPlaceNoSqlService;
        private ISiteContentNoSqlService _siteContentNoSqlService;

        public RegisterConfirmationModel(UserManager<IdentityUser> userManager, IEmailSender sender,
            IMarketplaceNoSqlService marketPlaceNoSqlService,
            ISiteContentNoSqlService siteContentNoSqlService) :
            base(marketPlaceNoSqlService, siteContentNoSqlService)
        {
            _userManager = userManager;
            _sender = sender;
            _marketPlaceNoSqlService = marketPlaceNoSqlService;
            _siteContentNoSqlService = siteContentNoSqlService;
        }

        public string Email { get; set; }

        public bool DisplayConfirmAccountLink { get; set; }

        public string EmailConfirmationUrl { get; set; }

        #region IPageModel Implementation
        [TempData]
        public string ErrorMessage { get; set; }
        public dynamic Data { get { return _data ?? (_data = new ExpandoObject()); } set { _data = value; } }
        SitePageType IPageModel.SitePage { get { return _page; } set { _page = value; } }

        public string PageTitle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string MarketplaceLogoImagePath => throw new NotImplementedException();

        IPrincipal IPageModel.User { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IDictionary<string, SiteContent> SiteContentBlock { get { return _siteContentBlock ?? new Dictionary<string, SiteContent>(); } set { _siteContentBlock = value; } }

        public SiteContent SiteContent { get; set; }
        #endregion

        public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
        {

            var model = CreateModel<DefaultModel>(page: SitePageType.Register, action: x =>
            {
                x.PageTitle = "MV Hair - Register Page";

            });

            SiteContentBlock = model.SiteContentBlock;

            if (email == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Unable to load user with email '{email}'.");
            }

            Email = email;
            // Once you add a real email sender, you should remove this code that lets you confirm the account
            DisplayConfirmAccountLink = false;
            if (DisplayConfirmAccountLink)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                EmailConfirmationUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                    protocol: Request.Scheme);
            }

            return Page();
        }
    }
}
