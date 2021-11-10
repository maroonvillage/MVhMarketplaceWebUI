using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using webui.Interfaces;
using webui.Models;

namespace marketplacewebcore.Areas.Identity.Pages.Account.Manage
{
    public partial class ContentModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IBasicIgApiService _basicApiSvc;
        private readonly ILogger<ContentModel> _logger;

        private string _accessToken;
        public ContentModel(UserManager<IdentityUser> userManager,
          IBasicIgApiService basicApiSvc,
          ILogger<ContentModel> logger,
          IConfiguration configuration
          )
        {
            _userManager = userManager;
            _basicApiSvc = basicApiSvc;
            _logger = logger;
            HasAccessToken = false;
            _accessToken = string.Empty;
            _configuration = configuration;

            MediaPosts = new QueryMediaResponse{
                Data = new List<ResponseData>()
            };
        
        }

        public string RequestCodeUri { get; set; }

        public bool HasAccessToken { get; set; }

        public QueryMediaResponse MediaPosts { get; set; }

        private async Task LoadAsync(IdentityUser user)
        {
            //TODO: check for any existing social network API connections 
            //TODO: ... and display all selected images/videos on this page
            var returnUri = _configuration.GetValue<string>("DefaultIgReturnUrl");
            //RequestCodeUri = await _basicApiSvc.GetAuthorizationUrl(returnUri);


            _accessToken = HttpContext.Request.Query["access_token"].ToString();
            if (!string.IsNullOrEmpty(_accessToken))
            {
                HasAccessToken = true;
                //MediaPosts = await _basicApiSvc.QueryIgUserMedia(_accessToken);
            }
            else
            {
                HasAccessToken = false;
            }
            await Task.FromResult(0);

        }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }


        public async Task<IActionResult> OnPostRequestTempCodeAsync()
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
            //await _basicApiSvc.RequestAuthorization("https://peaceful-bayou-22795.herokuapp.com/");

            return RedirectToPage();
        }



    }
}