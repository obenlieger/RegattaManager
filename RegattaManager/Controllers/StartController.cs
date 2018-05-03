using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RegattaManager.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RegattaManager.Controllers
{
    [Authorize]
    public class StartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? id)
        {
            var model = _context.Races.Include(e => e.Boatclass).Include(e => e.Oldclass).Include(e => e.Raceclass).Include(e => e.Regatta).Include(e => e.Racestatus).Include(e => e.Startboats).Where(e => e.RacestatusId == 1).OrderBy(e => e.Starttime).FirstOrDefault();
            ViewBag.startboats = _context.Startboats.Include(e => e.Club).Include(e => e.Startboatstatus).OrderBy(e => e.Startslot);
            ViewBag.startboatmembers = _context.StartboatMembers;
            ViewBag.members = _context.Members;
            ViewBag.raceid = id;
            ViewBag.currentTime = System.DateTime.Now;
            ViewBag.ReadyRaces = new SelectList(_context.Races.Include(e => e.Boatclass).Include(e => e.Oldclass).Include(e => e.Raceclass).Include(e => e.Regatta).Include(e => e.Racestatus).Include(e => e.Startboats).Where(e => e.RacestatusId == 1).OrderBy(e => e.Starttime).ToList(), "RaceId", "Starttime");
            ViewBag.ReadyRacesCount = _context.Races.Include(e => e.Boatclass).Include(e => e.Oldclass).Include(e => e.Raceclass).Include(e => e.Regatta).Include(e => e.Racestatus).Include(e => e.Startboats).Where(e => e.RacestatusId == 1).OrderBy(e => e.Starttime).Count();

            if(id != null)
            {
                model = _context.Races.Include(e => e.Boatclass).Include(e => e.Oldclass).Include(e => e.Raceclass).Include(e => e.Regatta).Include(e => e.Racestatus).Include(e => e.Startboats).Where(e => e.RacestatusId == 1).OrderBy(e => e.Starttime).FirstOrDefault(e => e.RaceId == id);

                ViewBag.allClicked = true;

                if (_context.Startboats.Any(e => e.RaceId == model.RaceId && e.StartboatstatusId == 6))
                {
                    ViewBag.allClicked = false;
                }

                return View(model);
            }

            if (model != null)
            {
                ViewBag.allClicked = true;

                if(_context.Startboats.Any(e => e.RaceId == model.RaceId && e.StartboatstatusId == 6))
                {
                    ViewBag.allClicked = false;
                }

                return View(model);
            }
            return View();
        }

        [HttpPost]
        public IActionResult VerifyStartboat(int id, int statusid)
        {
            var startboat = _context.Startboats.FirstOrDefault(e => e.StartboatId == id);
            startboat.StartboatstatusId = statusid;
            if (ModelState.IsValid)
            {
                _context.Entry(startboat).State = EntityState.Modified;
                _context.SaveChanges();
            }
            
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult StartRace(int id)
        {
            var race = _context.Races.FirstOrDefault(e => e.RaceId == id);
            
            race.RacestatusId = 2;
            race.Realstarttime = System.DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Entry(race).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}