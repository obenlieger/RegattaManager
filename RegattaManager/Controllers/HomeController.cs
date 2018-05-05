using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RegattaManager.Models;
using RegattaManager.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RegattaManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        public IActionResult Index(string searchLastName, string All, string ZE, int ClubId)
        {
            ViewData["CurrentFilter"] = searchLastName;

            var model = _context.Races.Include(e => e.Boatclass).Include(e => e.Oldclass).Include(e => e.Raceclass).Include(e => e.Racestatus).Include(e => e.Startboats).Where(e => e.RacestatusId == 1).OrderBy(e => e.Starttime).Take(10);

            ViewBag.startboats = _context.Startboats.Include(e => e.Club).OrderBy(e => e.Startslot);
            ViewBag.startboatmembers = _context.StartboatMembers;
            ViewBag.members = _context.Members.Include(e => e.Club);
            ViewBag.ClubId = new SelectList(_context.Clubs.OrderBy(e => e.Name), "ClubId", "Name");

            if (!String.IsNullOrEmpty(searchLastName))
            {
                var sbm = _context.StartboatMembers.Where(e => e.Member.LastName.ToLower().Contains(searchLastName.ToLower()));
                var sb = new List<Startboat>();
                var races = new List<Race>();
                var allRaces = _context.Races.Include(e => e.Boatclass).Include(e => e.Oldclass).Include(e => e.Raceclass).Include(e => e.Racestatus).Include(e => e.Startboats).Where(e => e.RacestatusId == 1).OrderBy(e => e.Starttime);

                foreach (var stbm in sbm)
                {
                    sb.Add(_context.Startboats.Include(e => e.StartboatMembers).Where(e => e.StartboatId == stbm.StartboatId).Single());
                }
                foreach (var sbsgl in sb)
                {
                    foreach (var r in allRaces)
                    {
                        if (r.RaceId == sbsgl.RaceId)
                        {
                            races.Add(r);
                        }
                    }
                }

                return View(races);
            }

            if (!String.IsNullOrEmpty(All))
            {
                ViewData["All"] = "1";
                var races = _context.Races.Include(e => e.Boatclass).Include(e => e.Oldclass).Include(e => e.Raceclass).Include(e => e.Racestatus).Include(e => e.Startboats).Where(e => e.RacestatusId == 1).OrderBy(e => e.Starttime);

                return View(races);
            }

            if (!String.IsNullOrEmpty(ZE))
            {
                ViewData["ZE"] = "1";
                var races = _context.Races.Include(e => e.Boatclass).Include(e => e.Oldclass).Include(e => e.Raceclass).Include(e => e.Racestatus).Include(e => e.Startboats).Include(e => e.RaceTyp).Where(e => e.RacestatusId == 1005).OrderBy(e => e.Starttime);

                return View(races);
            }

            return View(model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
