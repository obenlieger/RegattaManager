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

        public IActionResult Index(string searchLastName, string All, string ZE, int? filterClubId)
        {
            var rid = 0;

            if(_context.Regattas.Where(e => e.Choosen == true).Any())
            { 
                rid = _context.Regattas.Where(e => e.Choosen == true).FirstOrDefault().RegattaId;
            }

            var regattaClubs = _context.RegattaClubs.Where(e => e.RegattaId == rid);

            ViewData["CurrentFilter"] = searchLastName;
            ViewData["filterClub"] = new SelectList(_context.Clubs.Where(e => regattaClubs.Select(i => i.ClubId).Contains(e.ClubId)),"ClubId","Name");

            var model = _context.Races.Include(e => e.Boatclass).Include(e => e.Oldclass).Include(e => e.Raceclass).Include(e => e.Racestatus).Include(e => e.Startboats).Where(e => e.RacestatusId == 1).OrderBy(e => e.Starttime).Take(10).ToList();

            ViewBag.startboats = _context.Startboats.Include(e => e.Club).OrderBy(e => e.Startslot).ToList();
            ViewBag.startboatmembers = _context.StartboatMembers.ToList();
            ViewBag.members = _context.Members.Include(e => e.Club).ToList();
            ViewBag.ClubId = new SelectList(_context.Clubs.OrderBy(e => e.Name), "ClubId", "Name");

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
                var allRaces = _context.Races.Include(e => e.Boatclass).Include(e => e.Oldclass).Include(e => e.Raceclass).Include(e => e.Racestatus).Include(e => e.Startboats).Where(e => e.RacestatusId == 1).OrderByDescending(e => e.Starttime).ToList();                

                if(String.IsNullOrEmpty(searchLastName) && filterClubId != null && filterClubId > 0)
                {
                    sb = _context.Startboats.Include(e => e.StartboatMembers).Where(e => e.ClubId == filterClubId).ToList();
                }                
                
                if(sb != null)
                {
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
                }                

                if(filterClubId != null && filterClubId > 0)
                {
                    ViewData["filterClub"] = new SelectList(_context.Clubs.Where(e => regattaClubs.Select(i => i.ClubId).Contains(e.ClubId)),"ClubId","Name",filterClubId);
                }

                return View(races);
            }

            if (!String.IsNullOrEmpty(All))
            {
                ViewData["All"] = "1";
                var races = _context.Races.Include(e => e.Boatclass).Include(e => e.Oldclass).Include(e => e.Raceclass).Include(e => e.Racestatus).Include(e => e.Startboats).Where(e => e.RacestatusId == 1).OrderBy(e => e.Starttime).ToList();

                return View(races);
            }

            if (!String.IsNullOrEmpty(ZE))
            {
                ViewData["ZE"] = "1";
                var races = _context.Races.Include(e => e.Boatclass).Include(e => e.Oldclass).Include(e => e.Raceclass).Include(e => e.Racestatus).Include(e => e.Startboats).Include(e => e.RaceTyp).Where(e => e.RacestatusId == 1005).OrderBy(e => e.Starttime).ToList();

                return View(races);
            }

            return View(model);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
