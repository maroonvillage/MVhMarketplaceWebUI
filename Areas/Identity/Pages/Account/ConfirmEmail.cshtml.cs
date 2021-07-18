using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using webui.Enums;
using webui.Interfaces;
using webui.Models;

namespace webui.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : RazorBasePageModel, IPageModel
    {
        private dynamic _data;
        private SitePageType _page = SitePageType.Unknown;
        private IDictionary<string, SiteContent> _siteContentBlock;
        private readonly UserManager<IdentityUser> _userManager;
        private IMarketplaceService _marketPlaceService;
        private ISiteContentService _siteContentService;

        public ConfirmEmailModel(UserManager<IdentityUser> userManager,
            IMarketplaceService marketPlaceService,
            ISiteContentService siteContentService) :
            base(marketPlaceService, siteContentService)
        {
            _marketPlaceService = marketPlaceService;
            _siteContentService = siteContentService;
            _userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }


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

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var model = CreateModel<DefaultModel>(page: SitePageType.Register, action: x =>
            {
                x.PageTitle = "MV Hair - Register Page";

            });
            SiteContentBlock = model.SiteContentBlock;


            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
            return Page();
        }
    }
}
