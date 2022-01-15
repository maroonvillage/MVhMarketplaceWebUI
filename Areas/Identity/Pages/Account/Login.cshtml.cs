using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using webui.Enums;
using webui.Interfaces;
using webui.Models;

namespace webui.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : RazorBasePageModel, IPageModel
    {
        private dynamic _data;
        private SitePageType _page = SitePageType.Unknown;
        private IDictionary<string, SiteContent> _siteContentBlock;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private IMarketplaceNoSqlService _marketPlaceNoSqlService;
        private ISiteContentNoSqlService _siteContentNoSqlService;

        public LoginModel(SignInManager<IdentityUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<IdentityUser> userManager,
            IMarketplaceNoSqlService marketPlaceNoSqlService,
            ISiteContentNoSqlService siteContentNoSqlService) :
            base(marketPlaceNoSqlService, siteContentNoSqlService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _marketPlaceNoSqlService = marketPlaceNoSqlService;
            _siteContentNoSqlService = siteContentNoSqlService;

        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }
        public dynamic Data { get { return _data ?? (_data = new ExpandoObject()); } set { _data = value; } }
        SitePageType IPageModel.SitePage { get { return _page; } set { _page = value; } }

        public string PageTitle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string MarketplaceLogoImagePath => throw new NotImplementedException();

        IPrincipal IPageModel.User { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IDictionary<string, SiteContent> SiteContentBlock { get { return _siteContentBlock ?? new Dictionary<string, SiteContent>(); } set { _siteContentBlock = value; } }

        public SiteContent SiteContent { get; set; }
        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }


        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            var model = CreateModel<DefaultModel>(page: SitePageType.Login, action: x =>
            {
                x.PageTitle = "MV Hair - Login Page";

            });

            //this.SiteContent = model.SiteContent;
            this.SiteContentBlock = model.SiteContentBlock;
            this.SiteContent = model.SiteContent;

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/Identity/Account/Login");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            
            ReturnUrl = returnUrl;

           return Page();

        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

    }
}
