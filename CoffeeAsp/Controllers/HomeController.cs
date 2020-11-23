using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CoffeeAsp.Models;
using CoffeeAsp.Data;
using Microsoft.EntityFrameworkCore;
using CoffeeAsp.ViewModel;

namespace CoffeeAsp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FrontContext _context;
        public HomeController(ILogger<HomeController> logger, FrontContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Sliders> slider = await _context.Sliders.ToListAsync();
            IEnumerable<Brands> brand = await _context.Brands.ToListAsync();
            IEnumerable<ColdMenu> coldMenu = await _context.ColdMenus.ToListAsync();
            IEnumerable<HotMenu> hotMenu = await _context.HotMenus.ToListAsync();
            HomeIndexVM homeIndexVM = new HomeIndexVM()
            {
                sliders = slider,
                brands = brand,
                coldMenus = coldMenu,
                hotMenus = hotMenu
            };
            return View(homeIndexVM);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
