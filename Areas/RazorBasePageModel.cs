﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webui.Common;
using webui.Enums;
using webui.Extensions;
using webui.Interfaces;
using webui.Models;

namespace webui.Areas
{
    public class RazorBasePageModel : PageModel, IModelBuilder
    {
        private Marketplace _marketPlace;
        private IMarketplaceNoSqlService _marketPlaceNoSqlService;
        private ISiteContentNoSqlService _siteContentNoSqlService;


       public RazorBasePageModel(IMarketplaceNoSqlService marketPlaceNoSqlService, ISiteContentNoSqlService siteContentNoSqlService)
        {
            //_context = context;
            _marketPlaceNoSqlService = marketPlaceNoSqlService;
            _siteContentNoSqlService = siteContentNoSqlService;

        }

        public Marketplace Marketplace
        {
            get
            {
                return _marketPlace ?? HttpContext.Session.Get<Marketplace>(WebProperties.SessionKeyName) ?? new Marketplace();
            }

            set
            {
                _marketPlace = value;
                HttpContext.Session.Set<Marketplace>(WebProperties.SessionKeyName, _marketPlace);
            }
        }

        public async override Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context,
                                                 PageHandlerExecutionDelegate next)
        {
            // Do post work.
            Initialize();
            await next.Invoke();
        }

        public void Initialize()
        {
            var marketPlaceIdCookie = HttpContext != null ? HttpContext.Request.Cookies["MarketplaceId"] : null;
            
            string marketPlaceId = null;
            if (marketPlaceIdCookie != null)
            {
                marketPlaceId = marketPlaceIdCookie;
            }
            var domain = HttpContext.Request.Host.Value;
            Marketplace = string.IsNullOrEmpty(marketPlaceId) ? _marketPlaceNoSqlService.GetMarketplaceByDomainAsync(domain).Result : _marketPlaceNoSqlService.GetMarketplaceByIdAsync(marketPlaceId).Result;
            // ERROR HERE IF MARKETPLACE IS NULL (NO MARKETPLACE DETECTED)
            if (Marketplace == null || string.IsNullOrEmpty(Marketplace.MarketplaceId))
            {
                Response.Redirect("/error/marketplace-not-configured");
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
                    pageMachineName = GetType().Name.Replace("Model", string.Empty);
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
                pageMachineName = GetType().Name.Replace("Model", string.Empty);
            }

            return _siteContentNoSqlService.GetSiteContentDictionary(Marketplace, pageMachineName);
        }


    }
}
