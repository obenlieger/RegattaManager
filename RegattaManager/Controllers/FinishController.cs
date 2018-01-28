using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RegattaManager.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace RegattaManager.Controllers
{
    [Authorize]
    public class FinishController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FinishController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? id)
        {
            var model = _context.Races.Include(e => e.Boatclass).Include(e => e.Oldclass).Include(e => e.Raceclass).Include(e => e.Regatta).Include(e => e.Racestatus).Include(e => e.Startboats).Where(e => e.RacestatusId == 2).OrderBy(e => e.Starttime).FirstOrDefault();

            if (id != null)
            {
                var model_id = _context.Races.Include(e => e.Boatclass).Include(e => e.Oldclass).Include(e => e.Raceclass).Include(e => e.Regatta).Include(e => e.Racestatus).Include(e => e.Startboats).Where(e => e.RacestatusId == 2).OrderBy(e => e.Starttime).Where(e => e.RaceId == id);
                return View(model_id);
            }

            ViewBag.startboats = _context.Startboats.Include(e => e.Club).Include(e => e.Startboatstatus).Where(e => e.StartboatstatusId != 3).OrderBy(e => e.Startslot);
            ViewBag.startboatmembers = _context.StartboatMembers;
            ViewBag.members = _context.Members;
            ViewBag.raceid = id;
            if (model != null)
            {
                ViewBag.pmmax = model.Startboats.Max(e => e.Placement) + 1;
            }
            else
            {
                ViewBag.pmmax = 0;
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult FinishStartboat(int id, int placement, int statusid)
        {
            var startboat = _context.Startboats.FirstOrDefault(e => e.StartboatId == id);
            startboat.StartboatstatusId = statusid;
            startboat.Placement = placement;
            if (ModelState.IsValid)
            {
                _context.Entry(startboat).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult FinishRace(int id)
        {
            var race = _context.Races.FirstOrDefault(e => e.RaceId == id);
            race.RacestatusId = 3;
            if (ModelState.IsValid)
            {
                _context.Entry(race).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}