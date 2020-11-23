using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeAsp.Data;
using CoffeeAsp.Models;
using CoffeeAsp.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoffeeAsp.Controllers
{
    public class MenuController : Controller
    {
        private readonly FrontContext _context;
        public MenuController(FrontContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<HotMenu> hotMenu = await _context.HotMenus.ToListAsync();
            IEnumerable<ColdMenu> coldMenu = await _context.ColdMenus.ToListAsync();
            MenuIndexVM menuIndexVM = new MenuIndexVM()
            {
                coldMenus = coldMenu,
                hotMenus = hotMenu
            };
            return View(menuIndexVM);
        }
    }
}
