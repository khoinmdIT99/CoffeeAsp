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
    public class AboutController : Controller
    {
        private readonly FrontContext _context;
        public AboutController(FrontContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Gallery> gallery = await _context.Galleries.ToListAsync();
            IEnumerable<Workers> worker = await _context.Workers.ToListAsync();
            IEnumerable<Counters> counter = await _context.Counters.ToListAsync();
            IEnumerable<Subcribes> subcribe = await _context.Subcribes.ToListAsync();
            IEnumerable<ColdMenu> coldMenu = await _context.ColdMenus.ToListAsync();
            IEnumerable<HotMenu> hotMenu = await _context.HotMenus.ToListAsync();
            IEnumerable<Products> product = await _context.Products.ToListAsync();
            AboutIndexVM aboutIndexVM = new AboutIndexVM()
            {
                galleries = gallery,
                workers = worker,
                counters = counter,
                subcribes = subcribe,
                coldMenus = coldMenu,
                hotMenus = hotMenu,
                products = product
            };
            return View(aboutIndexVM);
        }
    }
}
