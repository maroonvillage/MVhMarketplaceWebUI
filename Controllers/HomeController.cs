using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using webui.Data;
using webui.Models;
using webui.Services;

namespace webui.Controllers
{
    public class HomeController : SiteControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMarketplaceService _marketPlaceService;
        private readonly ISiteContentService _siteContentService;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger,IMarketplaceService marketPlaceService, ISiteContentService siteContentService) :
            base(context, marketPlaceService, siteContentService)
        {
            _context = context;
            _marketPlaceService = marketPlaceService;
            _siteContentService = siteContentService;

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
