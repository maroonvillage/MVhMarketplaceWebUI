﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using webui.Data;
using webui.Enums;
using webui.Interfaces;
using webui.Models;
using webui.Services;

namespace webui.Controllers
{
    public class HomeController : SiteControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMarketplaceService _marketPlaceService;
        private readonly ISiteContentService _siteContentService;
        private readonly IServiceProvider _service;
        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger,
                IMarketplaceService marketPlaceService, ISiteContentService siteContentService, IServiceProvider serviceProvider) :
            base(context, marketPlaceService, siteContentService)
        {
            _context = context;
            _marketPlaceService = marketPlaceService;
            _siteContentService = siteContentService;
            _service = serviceProvider;

        }
        public IActionResult Index()
        {
            if (Marketplace.MarketplaceId == 0) return RedirectToAction("Error");

            HomeModel model = CreateModel<HomeModel>(page: SitePageType.Home, action: x =>
            {
                x.PageTitle = "MV Hair - Home Page";
                

            });

            return View(model);
        }

        public IActionResult Privacy()
        {
            HomeModel model = CreateModel<HomeModel>(page: SitePageType.Home, action: x =>
            {
                x.PageTitle = "MV Hair - Privacy Page";


            });

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
