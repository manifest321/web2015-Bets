using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BetsTheBest.Models;

namespace BetsTheBest.Controllers
{
    public class ModeratorController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        List<string> sportTournament = new List<string>()
        {
            "Лига чемпионов",
            "Лига европы",
            "АПЛ",
            "Бундос Лига",
            "РФПЛ",
        };

        // GET: Moderator
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult addEvent(SportEvent toAdd)
        {
            db.events.Add(toAdd);
            db.SaveChanges();
            
            return RedirectToAction("addEvent");
        }

        [HttpGet]
        public ActionResult addResult()
        {
            return View();
        }

        [HttpPost]
        public ActionResult addResult(Results r)
        {
            db.results.Add(r); db.SaveChanges();
            return RedirectToAction("Index","Moderator");
        }

        [HttpGet]
        public ActionResult addEvent()
        {
            ViewBag.gameType = new SelectList(eventTypes.toList()).ToList();
            ViewBag.gameState = new SelectList(eventState.toList()).ToList();
            ViewBag.sportTour = new SelectList(sportTournament).ToList();
            return View();
        }
        
        [HttpPost]
        public ActionResult changeEventState(string state, int eventId)
        {
            SportEvent current = db.events.Find(eventId);
            if (state != eventState.live && state != eventState.preview && state != eventState.finished)
            {
                TempData["badState"] = "неопознанное состояние события";
                return RedirectToAction("Index", "Home");
            }

            if (state == eventState.preview)
            {
                TempData["badState"] = "невозможно перевести событие в состояние ожидания";
                return RedirectToAction("Index", "Home");
            }

            if (state == eventState.live)
            {
                current.state = eventState.live;
                db.SaveChanges();
                TempData["goodState"] = "состояние изменено";
                return RedirectToAction("Index", "Home");
            }

            if (state == eventState.finished)
            {
                current.state = eventState.finished;
                updateBets(current);
                db.SaveChanges();
                TempData["goodState"] = "состояние изменено";
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "View");
        }

        private void updateBets(SportEvent e)
        {
            var bets = db.bets
                .Where(b => b.betEventId == e.Id)
                .Select(b => b);
            var users = db.Users.ToList();
            var results = db.results.ToList();
            foreach (Bet b in bets)
            {
                if (b.state != "waiting") continue;
                string type = b.betType;
                int typeId = 0; float coff = 0;
                switch (type)
                {
                    case "П1": typeId = 0; coff = e.firstWin; break;
                    case "П2": typeId = 1; coff = e.secondWin; break;
                    case "Н": typeId = 2; coff = e.gameTie; break;
                    case "Н1": typeId = 3; coff = e.tieByFirst; break;
                    case "Н2": typeId = 4; coff = e.tieBySecond; break;
                    case "НН": typeId = 5; coff = e.overallTie; break;
                    case "Тотал": typeId = 6; coff = e.total; break;
                    case "Б": typeId = 7; coff = e.b; break;
                    case "М": typeId = 8; coff = e.m; break;
                }
                ApplicationUser user = users.FirstOrDefault(u => u.Id == b.userId);
                Results result = results.FirstOrDefault(r => r.eId == e.Id);
                if (user != null && result != null)
                {
                    if (((result.result >> typeId) & 1) == 1)
                    {
                        user.userAmount += (b.amount * coff);
                        b.state = "won";
                    }
                    else b.state = "lost";
                }
            }
        }

        [HttpGet]
        public ActionResult banUser()
        {
            return View(db.Users);
        }


        [HttpPost]
        public ActionResult banUser(string uId, string operation)
        {
            try
            {
                ApplicationUser toBan = db.Users.Single(u => u.Id == uId);
                if (operation == "ban") toBan.userStatus = UserSatus.baned;
                if (operation == "unban") toBan.userStatus = UserSatus.active;
                db.SaveChanges();
            }
            catch {
                TempData["badState"] = "Ошибка во время операции!";
            }
            return RedirectToAction("banUser");
        }
        
    }
}