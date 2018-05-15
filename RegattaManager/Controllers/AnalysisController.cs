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
                var clubs = _context.Clubs.Where(e => regattaclubs.Select(i => i.ClubId).Contains(e.ClubId)).ToList();
                var reportedStartboats = _context.ReportedStartboats.Include(e => e.ReportedRace).ThenInclude(e => e.Competition).ToList();
                var startingFees = _context.StartingFees.ToList();
                var oldclasses = _context.Oldclasses.ToList();

                ViewBag.reportedStartboats = reportedStartboats;
                ViewBag.startingFees = startingFees;
                ViewBag.oldclasses = oldclasses;
                ViewBag.rid = rid;

                return View(clubs);
            }

            return NotFound();
        }
    }
}