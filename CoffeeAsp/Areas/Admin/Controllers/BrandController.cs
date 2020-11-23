using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeAsp.Data;
using CoffeeAsp.Extensions;
using CoffeeAsp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoffeeAsp.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize(Roles = "Admin,Moderator")]
    public class BrandController : Controller
    {
        private readonly FrontContext _context;
        private readonly IWebHostEnvironment _env;
        public BrandController(FrontContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index(int? page = 1)
        {
            int skipCount = ((int)page - 1) * 3;
            var brand = await _context.Brands.OrderBy(b => b.Id).Skip(skipCount).Take(3).ToListAsync();
            ViewData["allBrandCount"] = _context.Brands.ToList().Count;
            ViewData["activePage"] = page;
            return View(brand);
        }
        public  IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Brands brands)
        {
            if (!ModelState.IsValid)
            {
                return View(brands);
            }
            if (brands.Photo == null)
            {
                ModelState.AddModelError("Photo", "Please select any image!");
                return View(brands);
            }
            if (!brands.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", "Image is not valid!");
                return View(brands);
            }
            brands.Image = await brands.Photo.SaveFileAsync(_env.WebRootPath);

            await _context.Brands.AddAsync(brands);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Brands brands = await _context.Brands.FindAsync(id);
            if (brands == null)
            {
                return NotFound();
            }
            return View(brands);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Brands brands = await _context.Brands.FindAsync(id);
            if (brands == null)
            {
                return NotFound();
            }
            return View(brands);
        }
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Brands brands = await _context.Brands.FindAsync(id);
            if (brands == null)
            {
                return NotFound();
            }
            string path = _env.WebRootPath + @"\image\" + brands.Image;
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            _context.Brands.Remove(brands);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Brands brands = await _context.Brands.FindAsync(id);
            if (brands == null)
            {
                return NotFound();
            }
            return View(brands);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Brands brands)
        {
            Brands brandDb = await _context.Brands.FindAsync(id);
            if (!ModelState.IsValid)
            {
                return View(brands);
            }
            if (brands.Photo != null)
            {
                if (brands.Photo.ContentType.Contains("image/"))
                {
                    string path = _env.WebRootPath + @"\image\" + brandDb.Image;
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    brandDb.Image = await brands.Photo.SaveFileAsync(_env.WebRootPath);
                }
                else
                {
                    ModelState.AddModelError("Photo", "Image is not valid!");
                    return View(brands);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
