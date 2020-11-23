using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeAsp.Models;
using CoffeeAsp.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeAsp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        public UsersController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View(_userManager.Users);  
        }
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AppUser appUser = await _userManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            return View(appUser);
        }
        public async Task<IActionResult> Delete(string userid, string role)
        {
            if (userid == null || role == null)
            {
                return NotFound();
            }

            AppUser appUser = await _userManager.FindByIdAsync(userid);

            if (appUser == null)
            {
                return NotFound();
            }

            IdentityResult identityResult = await _userManager.RemoveFromRoleAsync(appUser, role);

            return RedirectToAction("Edit", new { id = userid });
        }
        public async Task<IActionResult> AddRole(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AppUser appUser = await _userManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            var userRoles = (await _userManager.GetRolesAsync(appUser)).ToList();
            var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();

            UserAddRoleVM userAddRoleVM = new UserAddRoleVM()
            {
                AppUser = appUser,
                Roles = allRoles.Except(userRoles)
            };

            return View(userAddRoleVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(string id, string role)
        {
            if (id == null || role == null)
            {
                return NotFound();
            }

            AppUser appUser = await _userManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            IdentityResult identityResult = await _userManager.AddToRoleAsync(appUser, role);

            if (identityResult.Succeeded)
            {
                return RedirectToAction(nameof(Index), new { id = id });
            }

            return RedirectToAction(nameof(Index), new { id = id });
        }
    }
}
