using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BetsTheBest.Models;

namespace BetsTheBest.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        private string CurrentUserId()
        {
            return db.Users.Single(a => a.UserName == User.Identity.Name).Id;
        }

        private float myBetCoefficient(Bet myBet, SportEvent sevent)
        {
            switch(myBet.betType)
            {
                case "П1":
                    return sevent.firstWin;
                case "П2":
                    return sevent.secondWin;
                case "Н":
                    return sevent.gameTie;
                case "НН":
                    return sevent.overallTie;
                case "Н1":
                    return sevent.tieByFirst;
                case "Н2":
                    return sevent.tieBySecond;
                case "Тотал":
                    return sevent.total;
                case "Б":
                    return sevent.b;
                case "М":
                    return sevent.m;
            }

            return 0.0f;
        }

        ApplicationUser CurrentUser()
        {
            return db.Users.Single(a => a.UserName == User.Identity.Name);
        }


        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                ApplicationUser curr = db.Users.Single(a => a.UserName == User.Identity.Name);
                ViewBag.userBill = curr.userAmount;
            }
            return View();
        }

        public ActionResult doBet(Bet toAdd)
        {
            try
            {
                ApplicationUser currentUser = CurrentUser();
                if (currentUser.userStatus == "baned")
                {
                    TempData["badState"] = "Вы не можете делать ставки, вы забанены";
                    return RedirectToAction("Index", "Home");
                }
                SportEvent current = db.events.Single(a => a.Id == toAdd.betEventId);
                if (current.state != eventState.preview && current.state != eventState.live)
                {
                    TempData["badState"] = "Невозможно сделать ставку на это событие";
                    return RedirectToAction("Index", "Home");
                }

                if (toAdd.amount > currentUser.userAmount)
                {
                    TempData["badState"] = "Недостаточно денег на балансе, пополните баланс";
                    return RedirectToAction("Index", "Home");
                }
                toAdd.userId = currentUser.Id;
                toAdd.state = "waiting";
                currentUser.userAmount -= toAdd.amount;
                db.bets.Add(toAdd);
                db.SaveChanges();
                TempData["goodState"] = "Ставка сделана!";
                return RedirectToAction("Index", "Home");
            }
            catch (ArgumentNullException e)
            {
                return HttpNotFound();
            }
        }

        [HttpGet]
        public ActionResult myPayments()
        {
            if (User.Identity.IsAuthenticated)
            {
                ApplicationUser curr = db.Users.Single(a => a.UserName == User.Identity.Name);
                ViewBag.userBill = curr.userAmount;
            }
            string uId = db.Users.Single(u=> u.UserName == User.Identity.Name).Id;
            return View(db.payments.Where(p => p.userId == uId).OrderByDescending(o=> o.paymentDate));
        }

        [HttpPost]
        public ActionResult newPayment(Payment toAdd)
        {
            ApplicationUser appUser = db.Users.Single(u => u.UserName == User.Identity.Name);
            
            if (appUser.userStatus == "baned")
            {
                TempData["badState"] = "Вы не можете делать платежи, вы забанены";
                return RedirectToAction("Index", "Home");
            }

            ApplicationUser currUser = db.Users.Single(u => u.UserName == User.Identity.Name);
            currUser.userAmount += toAdd.amount;
            toAdd.paymentDate = DateTime.Now;
            toAdd.userId = appUser.Id;
            db.payments.Add(toAdd);
            db.SaveChanges();
            return RedirectToAction("myPayments");
        }

        [HttpGet]
        public ActionResult myBets()
        {
            if (User.Identity.IsAuthenticated)
            {
                ApplicationUser curr = db.Users.Single(a => a.UserName == User.Identity.Name);
                ViewBag.userBill = curr.userAmount;
            }
            string uId = CurrentUserId();
            IEnumerable<BetEventViewModel> my = db.bets.Where(a => a.userId == uId).Join(db.events, f => f.betEventId, s => s.Id, (f, s) => new BetEventViewModel()
            { currBet = f, currEvent = s }).ToList();

            foreach(BetEventViewModel item in my)
            {
                item.betСoefficient = myBetCoefficient(item.currBet, item.currEvent);
                
            }

            return View(my);
        }

        [HttpGet]
        public ActionResult myMessages()
        {
            ApplicationUser curr = CurrentUser();
            return View(db.messages
                            .Where(m => m.src==curr.Email || m.dest==curr.Email));
        }

        [HttpPost]
        public ActionResult myMessages(string dest, string text)
        {
            if (text.Length > 0)
            {
                Messages m = new Messages();
                m.date = DateTime.Now;
                m.src = CurrentUser().Email;
                m.dest = dest;
                m.text = text;
                db.messages.Add(m);
                db.SaveChanges();
            }
            return RedirectToAction("myMessages");
        }
    }
}