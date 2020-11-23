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
    public class WorkerController : Controller
    {
        private readonly FrontContext _context;
        private readonly IWebHostEnvironment _env;
        public WorkerController(FrontContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index(int? page = 1)
        {
            int skipCount = ((int)page - 1) * 3;
            var workers = await _context.Workers.OrderBy(w => w.Id).Skip(skipCount).Take(3).ToListAsync();
            ViewData["allWorkerCount"] = _context.Workers.ToList().Count;
            ViewData["activePage"] = page;
            return View(workers);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Workers workers)
        {
            if (!ModelState.IsValid)
            {
                return View(workers);
            }
            if (workers.Photo == null)
            {
                ModelState.AddModelError("Photo", "Please select any image!");
                return View(workers);
            }
            if (!workers.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", "Image is not valid!");
                return View(workers);
            }

            workers.Image = await workers.Photo.SaveFileAsync(_env.WebRootPath);

            await _context.Workers.AddAsync(workers);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Deatil(int? id)
        {
            Workers workers = await _context.Workers.FindAsync(id);
            if (id == null)
            {
                return NotFound();
            }
            if (workers == null)
            {
                return NotFound();
            }
            return View(workers);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            Workers workers = await _context.Workers.FindAsync(id);
            if (id == null)
            {
                return NotFound();
            }
            if (workers == null)
            {
                return NotFound();
            }
            return View(workers);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Workers workers)
        {
            Workers workerdb = await _context.Workers.FindAsync(id);
            if (!ModelState.IsValid)
            {
                return View(workers);
            }
            if (workers.Photo != null)
            {
                if (workers.Photo.ContentType.Contains("image/"))
                {
                    string path = _env.WebRootPath + @"\image\" + workerdb.Image;
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }

                    workerdb.Image = await workers.Photo.SaveFileAsync(_env.WebRootPath);
                }
                else
                {
                    ModelState.AddModelError("Photo", "Image is not valid!");
                    return View(workers);
                }
            }

            workerdb.Name = workers.Name;
            workerdb.Job = workers.Job;
            workerdb.Instagram = workers.Instagram;
            workerdb.Facebook = workers.Facebook;
            workerdb.Twitter = workers.Twitter;
            workerdb.Tymblr = workers.Tymblr;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int? id)
        {
            Workers workers = await _context.Workers.FindAsync(id);
            if (id == null)
            {
                return NotFound();
            }
            if (workers == null)
            {
                return NotFound();
            }
            return View(workers);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int? id)
        {
            Workers workers = await _context.Workers.FindAsync(id);
            if (id == null)
            {
                return NotFound();
            }
            if (workers == null)
            {
                return NotFound();
            }

            string path = _env.WebRootPath + @"\image\" + workers.Image;

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            _context.Workers.Remove(workers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
