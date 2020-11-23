using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using CoffeeAsp.Models;
using CoffeeAsp.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace CoffeeAsp.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Resgitration()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Resgitration(RegistrVM registrVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registrVM);
            }

            AppUser appUser = new AppUser()
            {
                UserName = registrVM.UserName,
                Email = registrVM.Email
            };

            IdentityResult identityResult = await _userManager.CreateAsync(appUser, registrVM.Password);

            if (identityResult.Succeeded)
            {
                return RedirectToAction(nameof(Login));
            }
            else
            {
                foreach (IdentityError identityError in identityResult.Errors)
                {
                    ModelState.AddModelError("", identityError.Description);
                }
                return View(registrVM);
            }
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }

            AppUser appUser = await _userManager.FindByEmailAsync(loginVM.Email);

            if (appUser == null)
            {
                ModelState.AddModelError("", "Email or Password is wrong!");
                return View(loginVM);
            }

            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(appUser, loginVM.Password, loginVM.RememberMe, true);

            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Email or Password is wrong!");
                return View(loginVM);
            }

            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM changePasswordVM)
        {
            if (!ModelState.IsValid)
            {
                return View(changePasswordVM);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var result = await _userManager.ChangePasswordAsync(user, changePasswordVM.CurrentPassword, changePasswordVM.NewPassword);

            if (!result.Succeeded)
            {
                foreach (IdentityError identityError in result.Errors)
                {
                    ModelState.AddModelError("", identityError.Description);
                }
                return View(changePasswordVM);
            }

            await _signInManager.RefreshSignInAsync(user);
            return RedirectToAction(nameof(Login));
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM forgotPasswordVM)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(forgotPasswordVM.Email);

                if (user != null && await _userManager.IsEmailConfirmedAsync(user))
                {
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                    var callBackUrl = Url.Action("ResetPassword", "Auth", new { email = forgotPasswordVM.Email, code = code }, protocol: HttpContext.Request.Scheme);

                    MailMessage mailMessage = new MailMessage();

                    mailMessage.From = new MailAddress("axmed.dev.13@gmail.com", "Admin");
                    mailMessage.To.Add(new MailAddress(forgotPasswordVM.Email));
                    mailMessage.Subject = "Reset Password";
                    mailMessage.IsBodyHtml = true;
                    mailMessage.Body = $"For reseting password click here: <a href='{callBackUrl}'>link</a>";

                    SmtpClient smtpClient = new SmtpClient("smtp.google.com");

                    smtpClient.Port = 587;
                    smtpClient.UseDefaultCredentials = true;
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new NetworkCredential("axmed.dev.13@gmail.com", "devilprince2012");
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.Send(mailMessage);

                    return View("ForgotPasswordConfirmation");
                }
                //return View("ForgotPasswordConfirmation");
            }
            return View(forgotPasswordVM);
        }
        public IActionResult ResetPassword(string code, string email)
        {
            if (code == null || email == null)
            {
                ModelState.AddModelError("", "Email or password invalid");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(resetPasswordVM.Email);

                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, resetPasswordVM.Code, resetPasswordVM.Password);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Login", "Auth");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }

                        return View(resetPasswordVM);
                    }
                }
                return RedirectToAction("Login", "Auth");
            }

            return View(resetPasswordVM);
        }
    }
}
