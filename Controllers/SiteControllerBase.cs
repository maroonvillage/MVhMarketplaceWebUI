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
    public class SiteControllerBase : Controller, IModelBuilder
    {
        public const string SessionKeyName = "_Marketplace";

        //protected webuiIdentityDbContext _context;

        private Marketplace _marketPlace;
        private IMarketplaceNoSqlService _marketPlaceNoSqlService;
        private ISiteContentNoSqlService _siteContentNoSqlService;

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

        public SiteControllerBase(IMarketplaceNoSqlService marketPlaceNoSqlService, ISiteContentNoSqlService siteContentNoSqlService)
        {
            _marketPlaceNoSqlService = marketPlaceNoSqlService;
            _siteContentNoSqlService = siteContentNoSqlService;
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
            Marketplace = marketPlaceId < 0 ? _marketPlaceNoSqlService.GetMarketplaceByDomainAsync(domain).Result : _marketPlaceNoSqlService.GetMarketplaceByIdAsync(Convert.ToString(marketPlaceId)).Result;
            // ERROR HERE IF MARKETPLACE IS NULL (NO MARKETPLACE DETECTED)
            if (Marketplace == null || string.IsNullOrEmpty(Marketplace.MarketplaceId))
            {
                Response.Redirect("/home/error/");
                return;
            }
           
        }


        public T CreateModel<T>(Action<T> action = null, string pageMachineName = null, SitePageType? page = null)
           where T : IPageModel, new()
        {
            return LoadModelData(new T(), action, pageMachineName, page);
        }


        public virtual T LoadModelData<T>(T model, Action<T> action = null, string pageMachineName = null,
            SitePageType? page = null)
           where T : IPageModel
        {
            model.Marketplace = Marketplace;

            //could not any other reference to the method below which is supposed to set the enumerated value based upon 
            // the name of the controller

            //model.Page = page ?? this.ToPage();
            model.SitePage = page ?? SitePageType.Unknown;
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

        public IDictionary<string, SiteContent> GetSiteContent(string pageMachineName = null)
        {
            if (string.IsNullOrWhiteSpace(pageMachineName))
            {
                pageMachineName = GetType().Name.Replace("Controller", string.Empty);
            }
            return _siteContentNoSqlService.GetSiteContentDictionary(Marketplace, pageMachineName);
        }
    }
}
