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
    public class ProductController : Controller
    {
        private readonly FrontContext _context;
        public ProductController(FrontContext context)
        {
            _context = context;
        }
        public IActionResult Index(string search = null, int? page = 1)
        {
            int skipCount = ((int)page - 1) * 9;
            var product = _context.Products.OrderBy(p => p.Id).Skip(skipCount).Take(9).ToList();
            if (search != null)
            {
                product = _context.Products.Where(p => p.Name.Contains(search)).OrderBy(p => p.Id).Skip(skipCount).Take(9).ToList();
            }
            ViewData["allProductCount"] = _context.Products.ToList().Count;
            ViewData["activePage"] = page;
            ProductIndexVM productIndexVM = new ProductIndexVM()
            {
                products = product
            };
            return View(productIndexVM);
        }
        public IActionResult Search(string search = null, int? page = 1)
        {
            int skipCount = ((int)page - 1) * 9;
            var product = _context.Products.OrderBy(p => p.Id).Skip(skipCount).Take(9).ToList();// belke 2 dene product var ona gore reaksiya vermir birini silek>?
            if (search != null)
            {
                product = _context.Products.Where(p => p.Name.Contains(search)).OrderBy(p => p.Id).Skip(skipCount).Take(9).ToList();
            }
            ViewData["allProductCount"] = _context.Products.ToList().Count;
            ViewData["activePage"] = page;
            ProductIndexVM productIndexVM = new ProductIndexVM()
            {
                products = product
            };
            return RedirectToAction(nameof(Index),new { search = search, page = 1});
        }
        public async Task<IActionResult> Detail(int? id)
        {
            Products products = await _context.Products.FindAsync(id);
            if (id == null) return NotFound();
            if (products == null) return NotFound();
            return View(products);
        }
    }
}
