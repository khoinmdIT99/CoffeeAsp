using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeAsp.Data;
using CoffeeAsp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;

namespace CoffeeAsp.Controllers
{
    public class CartController : Controller
    {
        private readonly FrontContext _context;
        private readonly UserManager<AppUser> _userManager;

        public static int count = 0;
        public static int quantity = 0;
        public static decimal multiple = 0;
        public static decimal totalPrice = 0;
        public CartController(FrontContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Add(int id)
        {
            Carts product = await _context.Products.FindAsync(id);

            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            product.AppUserId = user.Id;

            string cart = HttpContext.Session.GetString("cart");
            List<Carts> products = new List<Carts>();
            if (cart != null)
            {
                products = JsonConvert.DeserializeObject<List<Carts>>(cart);
            }
            var selected = products.FirstOrDefault(p => p.Id == id);
            if (selected == null)
            {
                products.Add(product);
            }
            else
            {
                selected.Quantity++;
            }
            count += 1;
            string serializedProducts = JsonConvert.SerializeObject(products);
            HttpContext.Session.SetString("cart", serializedProducts);
            return Ok(new
            {
                status = 200,
                data = count,
                message = "you added product to basket successfully!"
            });
        }

        public async Task<IActionResult> AddSingleProduct(int id)
        {
            Carts product = await _context.Products.FindAsync(id);

            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            product.AppUserId = user.Id;

            string cart = HttpContext.Session.GetString("cart");
            List<Carts> products = new List<Carts>();
            if (cart != null)
            {
                products = JsonConvert.DeserializeObject<List<Carts>>(cart);
            }
            var selected = products.FirstOrDefault(p => p.Id == id);
            if (selected == null)
            {
                products.Add(product);
            }
            else
            {
                selected.Quantity++;
            }
            count += 1;
            string serializedProducts = JsonConvert.SerializeObject(products);
            HttpContext.Session.SetString("cart", serializedProducts);
            return Ok(new
            {
                status = 200,
                data = count,
                message = "you added product to basket successfully!"
            });
        }

        public async Task<IActionResult> Increment(int id)
        {
            //totalPrice = 0;
            Carts product = await _context.Products.FindAsync(id);
            string cart = HttpContext.Session.GetString("cart");
            List<Carts> products = new List<Carts>();
            if (cart != null)
            {
                products = JsonConvert.DeserializeObject<List<Carts>>(cart);
            }
            var selected = products.FirstOrDefault(p => p.Id == id);
            if (selected == null)
            {
                products.Add(product);
            }
            else
            {
                selected.Quantity++;
            }
            count += 1;
            if (selected.HasDiscount == true)
            {
                multiple = selected.Quantity * (decimal)selected.DiscountPrice;
            }
            else
            {
                multiple = selected.Quantity * selected.Price;
            }

            string serializedProducts = JsonConvert.SerializeObject(products);
            HttpContext.Session.SetString("cart", serializedProducts);
            return Ok(new
            {
                status = 200,
                data = count,
                multipleProduct = multiple,
                totalProduct = products.Sum(item => item.HasDiscount == true ? item.Quantity * item.DiscountPrice : item.Price * item.Quantity),
                productQuantity = selected.Quantity++,
                message = "you added product to basket successfully!"
            });
        }
        public async Task<IActionResult> Decrement(int id)
        {
            Carts product = await _context.Products.FindAsync(id);
            string cart = HttpContext.Session.GetString("cart");
            List<Carts> products = new List<Carts>();
            if (cart != null)
            {
                products = JsonConvert.DeserializeObject<List<Carts>>(cart);
            }

            var selected = products.FirstOrDefault(p => p.Id == id);

            if (selected != null && selected.Quantity > 1)
            {
                selected.Quantity--;
                count -= 1;
            }

            if (selected.HasDiscount == true)
            {
                multiple = selected.Quantity * (decimal)selected.DiscountPrice;
            }
            else
            {
                multiple = selected.Quantity * selected.Price;
            }

            string serializedProducts = JsonConvert.SerializeObject(products);
            HttpContext.Session.SetString("cart", serializedProducts);

            return Ok(new
            {
                status = 200,
                data = count,
                multipleProduct = multiple,
                totalProduct = products.Sum(item => item.HasDiscount == true ? item.Quantity * item.DiscountPrice : item.Price * item.Quantity),
                productQuantity = selected.Quantity--,
                message = "you added product to basket successfully!"
            });
        }
        public IActionResult Cart()
        {
            string cart = HttpContext.Session.GetString("cart");
            List<Carts> products = new List<Carts>();
            if (cart != null)
            {
                products = JsonConvert.DeserializeObject<List<Carts>>(cart);
            }
            return View(products);
        }
        public async Task<IActionResult> Remove(int id)
        {
            string cart = HttpContext.Session.GetString("cart");
            List<Carts> products = new List<Carts>();
            if (cart != null)
            {
                products = JsonConvert.DeserializeObject<List<Carts>>(cart);
            }
            Carts product = products.First(p => p.Id == id);
            products.Remove(product);
            count -= product.Quantity;

            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            product.AppUserId = user.Id;

            string serializedProducts = JsonConvert.SerializeObject(products);
            HttpContext.Session.SetString("cart", serializedProducts);

            return Ok(new
            {
                status = 200,
                data = count,
                totalProduct = products.Sum(item => item.HasDiscount == true ? item.Quantity * item.DiscountPrice : item.Price * item.Quantity),
                message = "you deleted products successfully!"
            });
        }
    }
}
