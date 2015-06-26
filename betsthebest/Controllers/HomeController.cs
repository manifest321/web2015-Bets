using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using BetsTheBest.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BetsTheBest.Controllers
{
    public class HomeController : Controller
    {
        protected ApplicationDbContext db = new ApplicationDbContext();
        protected UserManager<ApplicationUser> userManager;

        public HomeController()
        {
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                ApplicationUser curr = db.Users.Single(a=> a.UserName == User.Identity.Name);
                ViewBag.userBill = curr.userAmount;
            }
            if (TempData["badState"] != null)
                ViewData["badState"] = TempData["badState"];

            if (TempData["goodState"] != null)
                ViewData["goodState"] = TempData["goodState"];
            return View(db.events);
        }

        public ActionResult About() { return View(); }
        public ActionResult Rules() { return View(); }

        public ActionResult Live()
        {
            if (User.Identity.IsAuthenticated)
            {
                ApplicationUser curr = db.Users.Single(a => a.UserName == User.Identity.Name);
                ViewBag.userBill = curr.userAmount;
            }
            if (TempData["badState"] != null)
                ViewData["badState"] = TempData["badState"];

            if (TempData["goodState"] != null)
                ViewData["goodState"] = TempData["goodState"];
            return View("Index", db.events.Where(a=> a.state == eventState.live));
        }

        public ActionResult Upcoming()
        {
            if (User.Identity.IsAuthenticated)
            {
                ApplicationUser curr = db.Users.Single(a => a.UserName == User.Identity.Name);
                ViewBag.userBill = curr.userAmount;
            }
            if (TempData["badState"] != null)
                ViewData["badState"] = TempData["badState"];

            if (TempData["goodState"] != null)
                ViewData["goodState"] = TempData["goodState"];
            return View("Index", db.events.Where(a => a.state == eventState.preview));
        }

        [HttpGet]
        public ActionResult Feedback() {
            return View();
        }

        [HttpPost]
        public ActionResult Feedback(string subj, string text)
        {
            if (subj.Length > 0 && text.Length > 0)
            {
                ApplicationUser curr = userManager.FindById(User.Identity.GetUserId());
                string username = (curr == null) ? "" : (curr.Email+" ");
                string msg = "Сообщение отправлено", dest = "goodState";
                try
                {
                    MailMessage mailmsg = new MailMessage();
                    mailmsg.To.Add("xtn_l@mail.ru");
                    MailAddress address = new MailAddress("manifest321@gmail.com");
                    mailmsg.From = address;
                    mailmsg.Subject = username+subj;
                    mailmsg.Body = text;

                    SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                    client.EnableSsl = true;
                    NetworkCredential credentials = new NetworkCredential("manifest321@gmail.com", "m9ra1@xh");
                    client.Credentials = credentials;
                    client.Send(mailmsg);
                }
                catch
                {
                    msg = "Ошибка при отправке сообщения"; dest = "badState";
                }
                TempData[dest] = msg;
            }
            else TempData["badState"] = "Не заполнены все поля!";
            return new ContentResult()
            {
                Content = "<script type=text/javascript>window.parent.location.replace('/');</script>",
                ContentType = "text/html"
            };
        }
    }
}