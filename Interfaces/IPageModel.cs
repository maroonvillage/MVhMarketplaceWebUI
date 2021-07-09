using System;
using System.Collections.Generic;
using System.Security.Principal;
using webui.Enums;
using webui.Models;

namespace webui.Interfaces
{
    public interface IPageModel
    {

        dynamic Data { get; set; }

        string ErrorMessage { get; set; }

        SitePageType Page { get; set; }

        string PageTitle { get; set; }

        Marketplace Marketplace { get; set; }

        string MarketplaceLogoImagePath { get; }

        SiteContent SiteContent { get; set; }

        IDictionary<string, SiteContent> SiteContentBlock { get; set; }

        IPrincipal User { get; set; }
    }
}
