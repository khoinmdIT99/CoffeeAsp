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
    public class GalleryController : Controller
    {
        private readonly FrontContext _context;
        private readonly IWebHostEnvironment _env;
        public GalleryController(FrontContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index(int? page = 1)
        {
            int skipCount = ((int)page - 1) * 3;
            var galleries = await _context.Galleries.OrderBy(g => g.Id).Skip(skipCount).Take(3).ToListAsync();
            ViewData["allGalleryCount"] = _context.Galleries.ToList().Count;
            ViewData["activePage"] = page;
            return View(galleries);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Gallery gallery)
        {
            if (ModelState.IsValid)
            {
                return View(gallery);
            }
            if (gallery.Photo == null)
            {
                ModelState.AddModelError("Photo", "Please select any image!");
                return View(gallery);
            }
            if (!gallery.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", "Image is not valid!");
                return View(gallery);
            }

            gallery.Image = await gallery.Photo.SaveFileAsync(_env.WebRootPath);

            await _context.Galleries.AddAsync(gallery);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Detail(int? id)
        {
            Gallery gallery = await _context.Galleries.FindAsync(id);
            if (id == null)
            {
                return NotFound();
            }
            if (gallery == null)
            {
                return NotFound();
            }
            return View(gallery);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            Gallery gallery = await _context.Galleries.FindAsync(id);
            if (id == null)
            {
                return NotFound();
            }
            if (gallery == null)
            {
                return NotFound();
            }
            return View(gallery);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int? id)
        {
            Gallery gallery = await _context.Galleries.FindAsync(id);
            if (id == null)
            {
                return NotFound();
            }
            if (gallery == null)
            {
                return NotFound();
            }
            string path = _env.WebRootPath + @"\image\" + gallery.Image;
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            _context.Galleries.Remove(gallery);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int? id)
        {
            Gallery gallery = await _context.Galleries.FindAsync(id);
            if (id == null)
            {
                return NotFound();
            }
            if (gallery == null)
            {
                return NotFound();
            }
            return View(gallery);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Gallery gallery)
        {
            Gallery galleryDb = await _context.Galleries.FindAsync(id);
            if (!ModelState.IsValid)
            {
                return View(gallery);
            }
            if (gallery.Photo != null)
            {
                if (gallery.Photo.ContentType.Contains("image/"))
                {
                    string path = _env.WebRootPath + @"\image\" + galleryDb.Image;
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }

                    galleryDb.Image = await gallery.Photo.SaveFileAsync(_env.WebRootPath);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
