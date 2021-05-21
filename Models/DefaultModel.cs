using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Security.Principal;
using webui.Enums;

namespace webui.Models
{
    [NotMapped]
    public class DefaultModel : DynamicObject
    {
        private Marketplace _marketPlace;
        private SitePageType _page = SitePageType.Unknown;
        private string _pageTitle;
        private IList<string> _validationMessages;
        private IDictionary<string, SiteContent> _siteContentBlock;
        private SiteSettings _siteSettings;
        public SiteContent SiteContent { get; set; }
        private readonly IConfiguration _configuration;
        private dynamic _data;
        private IServiceProvider _serviceProvider;

        /// <summary>
        /// 
        /// </summary>
        public dynamic Data { get { return _data ?? (_data = new ExpandoObject()); } set { _data = value; } }


        /// <summary>
        /// A main error message for any given page
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Page to be displayed
        /// </summary>
        public SitePageType Page { get { return _page; } set { _page = value; } }

        /// <summary>
        /// Title for the page to be rendered inside &lt;title&gt;&lt;/title&gt;
        /// </summary>
        public string PageTitle
        {
            get { return _pageTitle ?? "No Page Title"; }
            set { _pageTitle = value; }
        }

        public string MarketplaceLogoImagePath
        {
            get

            {
                var imgBasePath = _configuration.GetValue(typeof(string), "ImageBasePath");
                var logoFolder = _configuration.GetValue(typeof(string), "MarketplaceImageLogoPath");

                var logoPath = !string.IsNullOrWhiteSpace(_marketPlace.HeaderLogo) ? $"{imgBasePath}{_marketPlace.MarketplaceId.ToString()}{logoFolder}{_marketPlace.HeaderLogo}" :
                        imgBasePath.ToString();

                return logoPath;
            }
        }
        /// <summary>
        /// Current logged in user or null if Anonymous
        /// </summary>
        public IPrincipal User { get; set; }

        public IDictionary<string, SiteContent> SiteContentBlock { get { return _siteContentBlock ?? new Dictionary<string, SiteContent>(); } set { _siteContentBlock = value; } }

        public Marketplace Marketplace { get { return _marketPlace ?? (_marketPlace = new Marketplace { MarketplaceId = -1 }); } set { _marketPlace = value; } }

        //public IServiceProvider ServiceProvider

        //{
        //    get { return _serviceProvider; }
        //    set { _serviceProvider = value; }

        //}
    }
}
