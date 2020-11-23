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
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class HotMenuController : Controller
    {
        private readonly FrontContext _context;
        private readonly IWebHostEnvironment _env;
        public HotMenuController(FrontContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index(int? page = 1)
        {
            int skipCount = ((int)page - 1) * 5;
            var hotmenu = await _context.HotMenus.OrderBy(h => h.Id).Skip(skipCount).Take(5).ToListAsync();
            ViewData["allHotCount"] = _context.HotMenus.ToList().Count;
            ViewData["activePage"] = page;
            return View(hotmenu);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HotMenu hotMenu)
        {
            if (!ModelState.IsValid)
            {
                return View(hotMenu);
            }
            if (hotMenu.Photo == null)
            {
                ModelState.AddModelError("Photo", "Please select any image!");
                return View(hotMenu);
            }
            if (!hotMenu.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", "Image is not valid!");
                return View(hotMenu);
            }
            hotMenu.Image = await hotMenu.Photo.SaveFileAsync(_env.WebRootPath);

            await _context.HotMenus.AddAsync(hotMenu);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Detail(int? id)
        {
            HotMenu hotMenu = await _context.HotMenus.FindAsync(id);
            if (id == null) return NotFound();
            if (hotMenu == null) return NotFound();
            return View(hotMenu);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            HotMenu hotMenu = await _context.HotMenus.FindAsync(id);
            if (id == null) return NotFound();
            if (hotMenu == null) return NotFound();
            return View(hotMenu);
        }
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int? id)
        {
            HotMenu hotMenu = await _context.HotMenus.FindAsync(id);
            if (id == null) return NotFound();
            if (hotMenu == null) return NotFound();
            string path = _env.WebRootPath + @"\image\" + hotMenu.Image;
            if (_context.HotMenus.ToList().Count > 1)
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                _context.HotMenus.Remove(hotMenu);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int? id)
        {
            HotMenu hotMenu = await _context.HotMenus.FindAsync(id);
            if (id == null) return NotFound();
            if (hotMenu == null) return NotFound();
            return View(hotMenu);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, HotMenu hotMenu)
        {
            HotMenu hotMenuDb = await _context.HotMenus.FindAsync(id);
            if (!ModelState.IsValid)
            {
                return View(hotMenu);
            }
            if (hotMenu.Photo != null)
            {
                if (hotMenu.Photo.ContentType.Contains("image/"))
                {
                    string path = _env.WebRootPath + @"\image\" + hotMenuDb.Image;
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }

                    hotMenuDb.Image = await hotMenu.Photo.SaveFileAsync(_env.WebRootPath);
                }
                else
                {
                    ModelState.AddModelError("Photo", "Image is not valid!");
                    return View(hotMenu);
                }
            }

            hotMenuDb.Name = hotMenu.Name;
            hotMenuDb.Price = hotMenu.Price;
            hotMenuDb.Ingredients = hotMenu.Ingredients;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
