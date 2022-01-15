using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using webui.Interfaces;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webui.Controllers
{


    public class ProfileController : SiteControllerBase
    {

        private readonly ILogger<HomeController> _logger;
        private readonly IMarketplaceNoSqlService _marketPlaceNoSqlService;
        private readonly ISiteContentNoSqlService _siteContentService;
        private readonly IServiceProvider _service;
        public ProfileController(ILogger<HomeController> logger,
        IMarketplaceNoSqlService marketPlaceNoSqlService, ISiteContentNoSqlService siteContentNoSqlService, IServiceProvider serviceProvider) :
    base(marketPlaceNoSqlService, siteContentNoSqlService)

        {
            _marketPlaceNoSqlService = marketPlaceNoSqlService;
        }


        // GET: /<controller>/
        public IActionResult Index()
        {
            //if (Marketplace.MarketplaceId == 0) return RedirectToAction("Error");

            //HomeModel model = CreateModel<HomeModel>(page: SitePageType.Home, action: x =>
            //{
            //    x.PageTitle = "MV Hair - Home Page";

            //});

            //return View(model);
            return View();
        }
    }
}
