using System;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using PerosnalSiteMVC.UI.Models;
using System.Web.Configuration;

namespace PerosnalSiteMVC.UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Classmates()
        {
            

            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
           

            return View();
        }

        [HttpPost]
        public ActionResult Contact(ContactViewModel cvm)
        {
            if (!ModelState.IsValid)
            {
                //Send the cvm back to the form with completed inputs
                return View(cvm);
            }
            //build the message
            string message = $"You have received an email from {cvm.Name} with a subject of {cvm.Subject}. Please respond to {cvm.Email} with your response to the following message: <br/>{cvm.Message}";
            //MailMessage - What sends the email
            MailMessage mm = new MailMessage("postmaster@joshua-foley.com", "JMFoley91@outlook.com", cvm.Subject, message);

            //MailMessage properties
            //Allow HTML in the email
            mm.IsBodyHtml = true;
            mm.Priority = MailPriority.High;
            //Respond to the sender, and not our website
            mm.ReplyToList.Add(cvm.Email);

            //SmtpClient - This is the info from the host that allows this to be sent
            SmtpClient client = new SmtpClient("mail.joshua-foley.com");
            client.Port = 8889;
            client.EnableSsl = false;

            //Client Credentials
            client.Credentials = new NetworkCredential(WebConfigurationManager.AppSettings["EmailUser"].ToString(), WebConfigurationManager.AppSettings["EmailPass"].ToString());

            //Try to send the email
            try
            {
                //attempt to send
                client.Send(mm);
            }
            catch (Exception ex)
            {
                ViewBag.CustomerMessage = $"Your message was unable to send, Please Try Again.:{ex.Message}<br/> {ex.StackTrace}";
                return View(cvm);
            }

            return View("EmailConfirmation", cvm);
        }
    }
}
