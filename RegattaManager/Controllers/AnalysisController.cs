using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RegattaManager.Data;
using RegattaManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace RegattaManager.Controllers
{
    [Authorize]
    public class AnalysisController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnalysisController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var rid = 0;

            if (_context.Regattas.Where(e => e.Choosen == true).Any())
            {
                rid = _context.Regattas.Where(e => e.Choosen == true).FirstOrDefault().RegattaId;
            }

            if(rid != 0)
            {
                var regattaclubs = _context.RegattaClubs.Where(e => e.RegattaId == rid).ToList();
                var clubs = _context.Clubs.Where(e => regattaclubs.Select(i => i.ClubId).Contains(e.ClubId)).OrderBy(e => e.Name).ToList();
                var reportedStartboats = _context.ReportedStartboats.Include(e => e.ReportedRace).ThenInclude(e => e.Competition).ToList();
                var startingFees = _context.StartingFees.ToList();
                var campingFees = _context.CampingFees.ToList();
                var oldclasses = _context.Oldclasses.ToList();

                ViewBag.reportedStartboats = reportedStartboats;
                ViewBag.startingFees = startingFees;
                ViewBag.oldclasses = oldclasses;
                ViewBag.rid = rid;

                return View(clubs);
            }

            return NotFound();
        }

        public IActionResult Details(int id)
        {
            var rid = 0;

            if (_context.Regattas.Where(e => e.Choosen == true).Any())
            {
                rid = _context.Regattas.Where(e => e.Choosen == true).FirstOrDefault().RegattaId;
            }

            var model = _context.Clubs.FirstOrDefault(e => e.ClubId == id);

            var reportedStartboats = _context.ReportedStartboats.Where(e => e.ClubId == id).ToList();
            var startingFees = _context.StartingFees.ToList();
            var oldclasses = _context.Oldclasses.ToList();

            ViewBag.reportedStartboats = reportedStartboats;
            ViewBag.startingFees = startingFees;
            ViewBag.oldclasses = oldclasses;
            ViewBag.rid = rid;

            return View();
        }

        public IActionResult Nachmeldungen()
        {
            var startboats = _context.Startboats.ToList();
            var model = _context.ReportedStartboats.Include(e => e.Club).Include(e => e.Regatta).Include(e => e.ReportedRace).Include(e => e.ReportedRace.Oldclass).Include(e => e.ReportedRace.Competition.Boatclasses).Include(e => e.ReportedRace.Competition.Raceclasses).Where(e => e.isLate == true && !startboats.Select(i => i.ReportedStartboatId).Contains(e.ReportedStartboatId)).OrderByDescending(e => e.NoStartslot).ThenBy(e => e.modifiedDate).ToList();
            var addedstartboats = _context.ReportedStartboats.Include(e => e.Club).Include(e => e.Regatta).Include(e => e.ReportedRace).Include(e => e.ReportedRace.Oldclass).Include(e => e.ReportedRace.Competition.Boatclasses).Include(e => e.ReportedRace.Competition.Raceclasses).Where(e => e.isLate == true && startboats.Select(i => i.ReportedStartboatId).Contains(e.ReportedStartboatId)).OrderByDescending(e => e.NoStartslot).ThenBy(e => e.modifiedDate).ToList();
            var reportedsbm = _context.ReportedStartboatMembers.Include(e => e.Member).ToList();
            var reportedsbs = _context.ReportedStartboatStandbys.Include(e => e.Member).ToList();

            ViewBag.rsbm = reportedsbm;
            ViewBag.rsbs = reportedsbs;
            ViewBag.addedstartboats = addedstartboats;

            return View(model);
        }
    }
}