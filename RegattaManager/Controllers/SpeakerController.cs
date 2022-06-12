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
    public class SpeakerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SpeakerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var rid = 0;

            if(_context.Regattas.Where(e => e.Choosen == true).Any())
            { 
                rid = _context.Regattas.Where(e => e.Choosen == true).FirstOrDefault().RegattaId;
            }

            ViewBag.regattachosen = rid;

            if (rid != 0)
            {
                var model = _context.Races.Include(e => e.Boatclass).Include(e => e.Oldclass).Include(e => e.Raceclass).Include(e => e.Regatta).Include(e => e.Racestatus).Include(e => e.Startboats).Include(e => e.RaceTyp).Where(e => e.RegattaId == rid && e.RacestatusId != 2 && e.RacestatusId != 1002 && e.RacestatusId != 1004 && e.RacestatusId != 1006 && e.ResultSpoken == false).OrderBy(e => e.Starttime).ToList();                
                  
                ViewBag.startboats = _context.Startboats.Include(e => e.Club).OrderBy(e => e.Placement).ThenBy(e => e.Startslot).ToList();
                ViewBag.startboatmembers = _context.StartboatMembers.ToList();
                ViewBag.members = _context.Members.Include(e => e.Club).ToList(); 
                ViewBag.disqsbs = _context.Startboats.Include(e => e.Club).Include(e => e.Startboatstatus).Where(e => e.Placement <= 0).OrderBy(e => e.Startslot).ToList();
                ViewBag.AllClubs = _context.Clubs.ToList();

                return View(model);
            }
            else
            {
                return View();
            }          
        }

        public IActionResult SetSpoken(int id)
        {
            var model = _context.Races.FirstOrDefault(e => e.RaceId == id);

            if(model != null)
            {
                model.Spoken = true;
                _context.Races.Update(model);
                _context.SaveChanges();
            }

            return Redirect(Url.RouteUrl(new { controller = "Speaker", action = "Index" }) + "#" + id);
        }

        public IActionResult SetResultSpoken(int id)
        {
            var model = _context.Races.FirstOrDefault(e => e.RaceId == id);

            if (model != null)
            {
                model.ResultSpoken = true;
                _context.Races.Update(model);
                _context.SaveChanges();
            }

            return Redirect(Url.RouteUrl(new { controller = "Speaker", action = "Index" }) + "#" + id);
        }
    }
}