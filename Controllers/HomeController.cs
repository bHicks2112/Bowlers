using Bowlers.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Bowlers.Controllers
{
    public class HomeController : Controller
    {
        private IBowlersRepository _repo { get; set; }


        //constructor
        public HomeController(IBowlersRepository temp)
        {
            _repo = temp;
        }

        public IActionResult Index(string bowlerTeam)
        {

            var blah = _repo.Bowlers
                .Include(x => x.Team)
                .Where(x => x.Team.TeamName == bowlerTeam || bowlerTeam == null)
                .OrderBy(x => x.BowlerLastName)
                .ToList();


            return View(blah);
        }

        // EDIT TASK
        [HttpGet]
        public IActionResult EditBowler(int bowlerid)
        {
            ViewBag.Teams = _repo.Teams.ToList();

            var bowler = _repo.Bowlers.Single(x => x.BowlerID == bowlerid);

            return View("AddBowler", bowler);
        }

        [HttpPost]
        public IActionResult EditBowler(Bowler b)
        {
            _repo.SaveBowler(b);

            return RedirectToAction("Index");
        }


        // Delete task
        [HttpGet]
        public IActionResult DeleteBowler(int bowlerid)
        {
            var bowler = _repo.Bowlers.Single(x => x.BowlerID == bowlerid);
            return View(bowler);
        }

        [HttpPost]
        public IActionResult DeleteBowler(Bowler b)
        {
            _repo.DeleteBowler(b);

            return RedirectToAction("Index");
        }


        // add 
        [HttpGet]
        public IActionResult AddBowler()
        {
            ViewBag.Bowlers = _repo.Bowlers.ToList();
            ViewBag.Teams = _repo.Teams.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult AddBowler(Bowler b)
        {
            int max = 0;
            foreach(var x in _repo.Bowlers)
            {
                if(max < x.BowlerID)
                {
                    max = x.BowlerID;
                }
            }

            b.BowlerID = max + 1;

            _repo.CreateBowler(b);

            return RedirectToAction("Index");
        }



    }
}


