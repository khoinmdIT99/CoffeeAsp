using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeAsp.Data;
using CoffeeAsp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using CoffeeAsp.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace CoffeeAsp.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize(Roles = "Admin,Moderator")]
    public class ProductController : Controller
    {
        private readonly FrontContext _context;
        private readonly IWebHostEnvironment _env;
        public ProductController(FrontContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int? page = 1)
        {
            int skipCount = ((int)page - 1) * 4;
            var product = _context.Products.OrderBy(p => p.Id).Skip(skipCount).Take(4).ToList();
            ViewData["allProductCount"] = _context.Products.ToList().Count;
            ViewData["activePage"] = page;
            return View(product);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Products products)
        {
            if (!ModelState.IsValid)
            {
                return View(products);
            }
            if (products.Photo == null)
            {
                ModelState.AddModelError("Photo", "Please select any image!");
                return View(products);
            }
            if (!products.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", "Image is not valid!");
                return View(products);
            }
            products.Image = await products.Photo.SaveFileAsync(_env.WebRootPath);
            await _context.Products.AddAsync(products);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Detail(int? id)
        {
            Products products = await _context.Products.FindAsync(id);
            if (id == null) return NotFound();
            if (products == null) return NotFound();
            return View(products);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            Products products = await _context.Products.FindAsync(id);
            if (id == null) return NotFound();
            if (products == null) return NotFound();
            return View(products);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int? id)
        {
            Products products = await _context.Products.FindAsync(id);
            if (id == null) return NotFound();
            if (products == null) return NotFound();
            string path = _env.WebRootPath + @"\image\" + products.Image;
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            _context.Products.Remove(products);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int? id)
        {
            Products products = await _context.Products.FindAsync(id);
            if (id == null) return NotFound();
            if (products == null) return NotFound();
            return View(products);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Products products)
        {
            Products productsDb = await _context.Products.FindAsync(id);
            if (!ModelState.IsValid)
            {
                return View(products);
            }
            if (products.Photo != null)
            {
                if (products.Photo.ContentType.Contains("image"))
                {
                    string path = _env.WebRootPath + @"\image\" + productsDb.Image;
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }

                    productsDb.Image = await products.Photo.SaveFileAsync(_env.WebRootPath);
                }
                else
                {
                    ModelState.AddModelError("Photo", "Image is not valid!");
                    return View(products);
                }
            }
            productsDb.Name = products.Name;
            productsDb.Price = products.Price;
            productsDb.HasDiscount = products.HasDiscount;
            productsDb.DiscountPrice = products.DiscountPrice;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
