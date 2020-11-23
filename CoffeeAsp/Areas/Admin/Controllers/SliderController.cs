using System;
using System.Collections.Generic;
using System.IO;
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
    public class SliderController : Controller
    {
        private readonly FrontContext _context;
        private readonly IWebHostEnvironment _env;
        public SliderController(FrontContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index(int? page = 1)
        {
            int skipCount = ((int)page - 1) * 3;
            var sliders = await _context.Sliders.OrderBy(s => s.Id).Skip(skipCount).Take(3).ToListAsync();
            ViewData["allSliderCount"] = _context.Sliders.ToList().Count;
            ViewData["activePage"] = page;
            return View(sliders);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Sliders sliders)
        {
            if (!ModelState.IsValid)
            {
                return View(sliders);
            }
            if (sliders.Photo == null)
            {
                ModelState.AddModelError("Photo", "Please select any image!");
                return View(sliders);
            }
            if (!sliders.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", "Image is not valid!");
                return View(sliders);
            }

            sliders.Image = await sliders.Photo.SaveFileAsync(_env.WebRootPath);

            await _context.Sliders.AddAsync(sliders);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return NotFound();
            Sliders sliders = await _context.Sliders.FindAsync(id);
            if (sliders == null) return NotFound();
            return View(sliders);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            Sliders sliders = await _context.Sliders.FindAsync(id);
            if (sliders == null) return NotFound();
            return View(sliders);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null) return NotFound();
            Sliders sliders = await _context.Sliders.FindAsync(id);
            if (sliders == null) return NotFound();

            string path = _env.WebRootPath + @"\image\" + sliders.Image;
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            _context.Sliders.Remove(sliders);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            Sliders sliders = await _context.Sliders.FindAsync(id);
            if (sliders == null) return NotFound();
            return View(sliders);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Sliders sliders)
        {
            Sliders sliderdb = await _context.Sliders.FindAsync(id);
            if (!ModelState.IsValid)
            {
                return View(sliders);
            }
            if (sliders.Photo != null)
            {
                if (sliders.Photo.ContentType.Contains("image/"))
                {
                    string path = _env.WebRootPath + @"\image\" + sliderdb.Image;

                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }

                    sliderdb.Image = await sliders.Photo.SaveFileAsync(_env.WebRootPath);
                }
                else
                {
                    ModelState.AddModelError("Photo", "Image is not valid!");
                    return View(sliders);
                }
            }

            sliderdb.Header = sliders.Header;
            sliderdb.Description = sliders.Description;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
