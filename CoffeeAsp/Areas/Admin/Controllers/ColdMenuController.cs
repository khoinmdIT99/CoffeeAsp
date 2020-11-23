using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeAsp.Data;
using CoffeeAsp.Models;
using CoffeeAsp.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CoffeeAsp.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize(Roles = "Admin,Moderator")]
    public class ColdMenuController : Controller
    {
        private readonly FrontContext _context;
        private readonly IWebHostEnvironment _env;
        public ColdMenuController(FrontContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int? page = 1)
        {
            int skipCount = ((int)page - 1) * 5;
            var coldMenu =  _context.ColdMenus.OrderBy(c=>c.Id).Skip(skipCount).Take(5).ToList();
            ViewData["allColdCount"] = _context.ColdMenus.ToList().Count;
            ViewData["activePage"] = page;
            return View(coldMenu);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ColdMenu coldMenu) 
        {
            if (!ModelState.IsValid)
            {
                return View(coldMenu);
            }
            if (coldMenu.Photo == null)
            {
                ModelState.AddModelError("Photo", "Please select any image!");
                return View(coldMenu);
            }
            if (!coldMenu.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", "Image is not valid!");
                return View(coldMenu);
            }
            coldMenu.Image = await coldMenu.Photo.SaveFileAsync(_env.WebRootPath);
            await _context.ColdMenus.AddAsync(coldMenu);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Detail(int? id)
        {
            ColdMenu coldMenu = await _context.ColdMenus.FindAsync(id);
            if (id == null) return NotFound();
            if (coldMenu == null) return NotFound();
            return View(coldMenu);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            ColdMenu coldMenu = await _context.ColdMenus.FindAsync(id);
            if (id == null) return NotFound();
            if (coldMenu == null) return NotFound();
            return View(coldMenu);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int? id)
        {
            ColdMenu coldMenu = await _context.ColdMenus.FindAsync(id);
            if (id == null) return NotFound();
            if (coldMenu == null) return NotFound();
            string path = _env.WebRootPath + @"\image\" + coldMenu.Image;
            if (_context.ColdMenus.ToList().Count > 1)
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                _context.ColdMenus.Remove(coldMenu);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int? id)
        {
            ColdMenu coldMenu = await _context.ColdMenus.FindAsync(id);
            if (id == null) return NotFound();
            if (coldMenu == null) return NotFound();
            return View(coldMenu);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, ColdMenu coldMenu)
        {
            ColdMenu coldMenuDb = await _context.ColdMenus.FindAsync(id);
            if (!ModelState.IsValid)
            {
                return View(coldMenu);
            }
            if (coldMenu.Photo != null)
            {
                if (coldMenu.Photo.ContentType.Contains("image/"))
                {
                    string path = _env.WebRootPath + @"\image\" + coldMenuDb.Image;
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    coldMenuDb.Image = await coldMenu.Photo.SaveFileAsync(_env.WebRootPath);
                }
                else
                {
                    ModelState.AddModelError("Photo", "Image is not valid!");
                    return View(coldMenu);
                }
            }
            coldMenuDb.Name = coldMenu.Name;
            coldMenuDb.Price = coldMenu.Price;
            coldMenuDb.Ingredients = coldMenu.Ingredients;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
