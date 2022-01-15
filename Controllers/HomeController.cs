using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using webui.Enums;
using webui.Interfaces;
using webui.Models;

namespace webui.Controllers
{
    public class HomeController : SiteControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMarketplaceNoSqlService _marketPlaceNoSqlService;
        private readonly ISiteContentNoSqlService _siteContentNoSqlService;
        private readonly IServiceProvider _service;
        public HomeController(ILogger<HomeController> logger,
                IMarketplaceNoSqlService marketPlaceNoSqlService, ISiteContentNoSqlService siteContentNoSqlService, IServiceProvider serviceProvider) :
            base(marketPlaceNoSqlService, siteContentNoSqlService)
        {
            //_context = context;
            _marketPlaceNoSqlService = marketPlaceNoSqlService;
            _siteContentNoSqlService = siteContentNoSqlService;
            _service = serviceProvider;

        }
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(Marketplace.MarketplaceId)) return RedirectToAction("Error");

            HomeModel model = CreateModel<HomeModel>(page: SitePageType.Home, action: x =>
            {
                x.PageTitle = "MV Hair - Home Page";

            });

            return View(model);
        }

        public IActionResult AboutUs()
        {
            HomeModel model = CreateModel<HomeModel>(page: SitePageType.Home, action: x =>
            {
                x.PageTitle = "MV Hair - About Us Page";
            });

            return View(model);
        }

        public IActionResult ContactUs()
        {
            HomeModel model = CreateModel<HomeModel>(page: SitePageType.Home, action: x =>
            {
                x.PageTitle = "MV Hair - Contact Us Page";
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

            //HomeModel model = CreateModel<HomeModel>(page: SitePageType.Error, action: x =>
            //{
            //    x.PageTitle = "MV Hair - Error Page";
            //});

            //return View(model);


        }
    }
}
