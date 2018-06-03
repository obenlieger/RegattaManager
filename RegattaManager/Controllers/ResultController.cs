using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RegattaManager.Data;
using RegattaManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RegattaManager.Controllers
{
    public class ResultController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ResultController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string searchLastName, string All, int? filterClubId)
        {
            var rid = 0;

            if(_context.Regattas.Where(e => e.Choosen == true).Any())
            { 
                rid = _context.Regattas.Where(e => e.Choosen == true).FirstOrDefault().RegattaId;
            }

            var regattaClubs = _context.RegattaClubs.Where(e => e.RegattaId == rid);

            ViewData["CurrentFilter"] = searchLastName;
            ViewData["filterClub"] = new SelectList(_context.Clubs.Where(e => regattaClubs.Select(i => i.ClubId).Contains(e.ClubId)),"ClubId","Name");

            var model = _context.Races.Include(e => e.Boatclass).Include(e => e.Oldclass).Include(e => e.Raceclass).Include(e => e.Racestatus).Include(e => e.Startboats).Where(e => e.RacestatusId == 3).OrderByDescending(e => e.Starttime).Take(10).ToList();
            ViewBag.startboats = _context.Startboats.Include(e => e.Club).Include(e => e.Startboatstatus).OrderBy(e => e.RaceId).ThenBy(e => e.Placement).ToList();
            ViewBag.disqsbs = _context.Startboats.Include(e => e.Club).Include(e => e.Startboatstatus).Where(e => e.Placement <= 0).OrderBy(e => e.Startslot).ToList();
            ViewBag.startboatmembers = _context.StartboatMembers.ToList();
            ViewBag.members = _context.Members.Include(e => e.Club).ToList();
            ViewBag.ThisYear = DateTime.Now.Year;
            
            ViewBag.clubs = _context.Clubs.Where(e => regattaClubs.Select(i => i.ClubId).Contains(e.ClubId)).ToList();

            if (!String.IsNullOrEmpty(searchLastName) || filterClubId != null)
            {
                var sbm = _context.StartboatMembers.ToList();
                var sb = new List<Startboat>();

                if(!String.IsNullOrEmpty(searchLastName))
                {
                    sbm = _context.StartboatMembers.Where(e => e.Member.LastName.ToLower().Contains(searchLastName.ToLower())).ToList(); 

                    foreach (var stbm in sbm)
                    {
                        if(filterClubId != null && filterClubId > 0)
                        {
                            sb.Add(_context.Startboats.Include(e => e.StartboatMembers).FirstOrDefault(e => e.StartboatId == stbm.StartboatId && e.ClubId == filterClubId));    
                        }
                        else
                        {
                            sb.Add(_context.Startboats.Include(e => e.StartboatMembers).FirstOrDefault(e => e.StartboatId == stbm.StartboatId));
                        }                        
                    }                   
                }                
                
                var races = new List<Race>();                

                if(String.IsNullOrEmpty(searchLastName) && filterClubId != null && filterClubId > 0)
                {
                    sb = _context.Startboats.Include(e => e.StartboatMembers).Where(e => e.ClubId == filterClubId).ToList();
                }

                races = _context.Races.Include(e => e.Boatclass).Include(e => e.Oldclass).Include(e => e.Raceclass).Include(e => e.Racestatus).Include(e => e.Startboats).Where(e => sb.Select(i => i.RaceId).Contains(e.RaceId) && e.RacestatusId == 3).Distinct().OrderByDescending(e => e.Starttime).ToList();

                return View(races);
            }

            if (!String.IsNullOrEmpty(All))
            {
                ViewData["All"] = "1";
                var races = _context.Races.Include(e => e.Boatclass).Include(e => e.Oldclass).Include(e => e.Raceclass).Include(e => e.Racestatus).Include(e => e.Startboats).Where(e => e.RacestatusId == 3).OrderByDescending(e => e.Starttime).ToList();

                return View(races);
            }

            return View(model);
        }

        public IActionResult TV()
        {
            var rid = 0;

            if (_context.Regattas.Where(e => e.Choosen == true).Any())
            {
                rid = _context.Regattas.Where(e => e.Choosen == true).FirstOrDefault().RegattaId;
            }

            ViewBag.raceresults = _context.Races.Include(e => e.Boatclass).Include(e => e.Oldclass).Include(e => e.Raceclass).Include(e => e.Racestatus).Include(e => e.Startboats).Where(e => e.RacestatusId == 3).OrderByDescending(e => e.Starttime).Take(6).ToList();
            ViewBag.startboats = _context.Startboats.Include(e => e.Club).Include(e => e.Startboatstatus).OrderBy(e => e.RaceId).ThenBy(e => e.Placement).ThenBy(e => e.Startslot).ToList();
            ViewBag.disqsbs = _context.Startboats.Include(e => e.Club).Include(e => e.Startboatstatus).Where(e => e.Placement <= 0).OrderBy(e => e.Startslot).ToList();
            ViewBag.startboatmembers = _context.StartboatMembers.ToList();
            ViewBag.members = _context.Members.Include(e => e.Club).ToList();
            ViewBag.raceplanned = _context.Races.Include(e => e.Boatclass).Include(e => e.Oldclass).Include(e => e.Raceclass).Include(e => e.Racestatus).Include(e => e.Startboats).Where(e => e.RacestatusId == 1005).OrderBy(e => e.Starttime).Take(6).ToList();

            var regattaClubs = _context.RegattaClubs.Where(e => e.RegattaId == rid);
            ViewBag.clubs = _context.Clubs.Where(e => regattaClubs.Select(i => i.ClubId).Contains(e.ClubId)).ToList();

            return View();
        }
    }
}