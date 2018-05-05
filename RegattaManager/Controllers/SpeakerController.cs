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

        public IActionResult Index(bool result)
        {
            var rid = 0;

            if(_context.Regattas.Where(e => e.Choosen == true).Any())
            { 
                rid = _context.Regattas.Where(e => e.Choosen == true).FirstOrDefault().RegattaId;
            }

            ViewBag.regattachosen = rid;

            if (rid != 0)
            {
                var model = _context.Races.Include(e => e.Boatclass).Include(e => e.Oldclass).Include(e => e.Raceclass).Include(e => e.Regatta).Include(e => e.Racestatus).Include(e => e.Startboats).Include(e => e.RaceTyp).Where(e => e.RacestatusId == 1).Where(e => e.RegattaId == rid).OrderBy(e => e.Spoken).ThenBy(e => e.Starttime);
                
                if(result == true)
                {
                    model = _context.Races.Include(e => e.Boatclass).Include(e => e.Oldclass).Include(e => e.Raceclass).Include(e => e.Regatta).Include(e => e.Racestatus).Include(e => e.Startboats).Include(e => e.RaceTyp).Where(e => e.RacestatusId == 3).Where(e => e.RegattaId == rid).OrderBy(e => e.Spoken).ThenBy(e => e.Starttime);
                }
                                
                ViewBag.startboats = _context.Startboats.Include(e => e.Club).OrderBy(e => e.Startslot);
                ViewBag.startboatmembers = _context.StartboatMembers;
                ViewBag.members = _context.Members.Include(e => e.Club); 
                ViewBag.result = result;               

                return View(model.ToList());
            }
            else
            {
                return View();
            }          
        }

        public IActionResult SetSpoken(int id, bool result)
        {
            var model = _context.Races.FirstOrDefault(e => e.RaceId == id);

            if(model != null)
            {
                model.Spoken = true;
                _context.Races.Update(model);
                _context.SaveChanges();
            }

            return RedirectToAction("Index", new { id = id, result = result});
        }
    }
}