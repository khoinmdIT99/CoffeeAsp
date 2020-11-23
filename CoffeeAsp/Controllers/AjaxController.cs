using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeAsp.Data;
using CoffeeAsp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoffeeAsp.Controllers
{
    public class AjaxController : Controller
    {
        private readonly FrontContext _context;
        public AjaxController(FrontContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Subscribe(string email)
        {
            if (await _context.Subcribes.AnyAsync(s => s.Email == email))
            {
                return Ok(new
                {
                    status = 400,
                    data = "",
                    message = "Error dublicate!"
                });
            }
            else
            {
                await _context.Subcribes.AddAsync(new Subcribes { Email = email });
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    status = 200,
                    data = "",
                    message = "You subscribed successfully!"
                }); 
            }
        }
    }
}
