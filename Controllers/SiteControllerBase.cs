using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webui.Enums;
using webui.Extensions;
using webui.Interfaces;
using webui.Models;
using webui.Services;

namespace webui.Controllers
{
    public class SiteControllerBase : Controller
    {
        public const string SessionKeyName = "_Marketplace";

        protected Data.ApplicationDbContext _context;

        private Marketplace _marketPlace;
        private IMarketplaceService _marketPlaceService;
        private ISiteContentService _siteContentService;

        public Marketplace Marketplace
        {
            get
            {
                return _marketPlace ?? HttpContext.Session.Get<Marketplace>(SessionKeyName) ?? new Marketplace();
            }

            set
            {
                _marketPlace = value;
                HttpContext.Session.Set<Marketplace>(SessionKeyName, _marketPlace);
            }
        }

        public SiteControllerBase(Data.ApplicationDbContext context, IMarketplaceService marketPlaceService, ISiteContentService siteContentService)
        {
            _context = context;
            _marketPlaceService = marketPlaceService;
            _siteContentService = siteContentService;
        }


        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            var marketPlaceIdCookie = HttpContext.Request.Cookies["MarketplaceId"];

            var marketPlaceId = -1;
            if (marketPlaceIdCookie != null)
            {
                int.TryParse(marketPlaceIdCookie, out marketPlaceId);
            }
            var domain = HttpContext.Request.Host.Value;
            Marketplace = marketPlaceId < 0 ? _marketPlaceService.GetMarketplaceByDomain(domain) : _marketPlaceService.GetMarketplaceById(marketPlaceId);
            // ERROR HERE IF MARKETPLACE IS NULL (NO MARKETPLACE DETECTED)
            if (Marketplace == null || Marketplace.MarketplaceId < 0)
            {
                Response.Redirect("/error/marketplace-not-configured");
                return;
            }
           

        }

        protected T CreateModel<T>(Action<T> action = null, string pageMachineName = null, SitePageType? page = null)
           where T : DefaultModel, new()
        {
            return LoadModelData(new T(), action, pageMachineName, page);
        }


        protected virtual T LoadModelData<T>(T model, Action<T> action = null, string pageMachineName = null,
            SitePageType? page = null)
           where T : DefaultModel
        {
            model.Marketplace = Marketplace;

            //could not any other reference to the method below which is supposed to set the enumerated value based upon 
            // the name of the controller

            //model.Page = page ?? this.ToPage();
            model.Page = page ?? SitePageType.Unknown;
            if (model.SiteContentBlock == null || model.SiteContentBlock.Count == 0)
            {
                if (string.IsNullOrWhiteSpace(pageMachineName))
                {
                    pageMachineName = GetType().Name.Replace("Controller", string.Empty);
                }

                model.SiteContentBlock = GetSiteContent(pageMachineName);
            }

            //if (HttpContext.User != null && HttpContext.User.Identity.IsAuthenticated && HttpContext.User is Client)
            //{
            //    model.User = HttpContext.User;
            //}

            if (action != null)
            {
                action(model);
            }

            return model;
        }

        protected IDictionary<string, SiteContent> GetSiteContent(string pageMachineName = null)
        {
            if (string.IsNullOrWhiteSpace(pageMachineName))
            {
                pageMachineName = GetType().Name.Replace("Controller", string.Empty);
            }
            return _siteContentService.GetSiteContentDictionary(Marketplace, pageMachineName);
        }
    }
}
