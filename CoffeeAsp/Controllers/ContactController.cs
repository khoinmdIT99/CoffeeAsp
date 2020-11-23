using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using CoffeeAsp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeAsp.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Contact(Contact contact)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(contact.Email, contact.Name);
            mailMessage.To.Add(new MailAddress("indigoboy2012@gmail.com"));
            mailMessage.Subject = contact.Subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = contact.Name + " " + contact.Content + " " + contact.Email;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.Port = 587;
            smtpClient.UseDefaultCredentials = true;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential("indigoboy2012@gmail.com", "axmedlezgin2012");
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Send(mailMessage);

            return RedirectToAction("Contact");
        }
    }
}
