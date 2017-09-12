using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RegattaManager.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RegattaManager.Controllers
{
    public class RaceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RaceController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var rid = _context.RegattaChosen.FirstOrDefault().RegattaId;

            if(rid != 0)
            { 
                var model = _context.Races.Include(e => e.Boatclass).Include(e => e.Oldclass).Include(e => e.Raceclass).Include(e => e.Regatta).Include(e => e.Racestatus).Include(e => e.Startboats).Where(e => e.RacestatusId == 1).Where(e => e.RegattaId == rid).OrderBy(e => e.Starttime);
                ViewBag.startboats = _context.Startboats.Include(e => e.Club).OrderBy(e => e.Startslot);
                ViewBag.startboatmembers = _context.StartboatMembers;
                ViewBag.members = _context.Members;

                return View(model.ToList());
            }
            else
            {
                return View();
            }            
        }

        public IActionResult Details(int id)
        {
            var rid = _context.RegattaChosen.FirstOrDefault().RegattaId;

            if(rid != 0)
            { 
                var model = _context.Races.Include(e => e.Regatta).Include(e => e.Boatclass).Include(e => e.Raceclass).Include(e => e.Oldclass).Where(e => e.RegattaId == rid).FirstOrDefault(e => e.RaceId == id);

                int yearnow = DateTime.Now.Year;
                int ageFrom = 0;
                int ageTo = yearnow - model.Oldclass.ToAge;

                if (model.Oldclass.FromAge == 0)
                {
                    ageFrom = yearnow - model.Oldclass.FromAge;
                }
                else if (model.Oldclass.FromAge == 7)
                {
                    ageFrom = yearnow - 0;
                }
                else if (model.Oldclass.FromAge == 8)
                {
                    ageFrom = yearnow - 8;
                }
                else if (model.Oldclass.FromAge == 9)
                {
                    ageFrom = yearnow - 9;
                }
                else if (model.Oldclass.FromAge == 10)
                {
                    ageFrom = yearnow - 7;
                }
                else if (model.Oldclass.FromAge == 11)
                {
                    ageFrom = yearnow - 11;
                }
                else if (model.Oldclass.FromAge == 12)
                {
                    ageFrom = yearnow - 12;
                }
                else if (model.Oldclass.FromAge == 13)
                {
                    ageFrom = yearnow - 9;
                }
                else if (model.Oldclass.FromAge == 14)
                {
                    ageFrom = yearnow - 13;
                }
                else if (model.Oldclass.FromAge == 15)
                {
                    ageFrom = yearnow - 13;
                }
                else if (model.Oldclass.FromAge == 17)
                {
                    ageFrom = yearnow - 12;
                }
                else if (model.Oldclass.FromAge == 19)
                {
                    ageFrom = yearnow - 17;
                    ageTo = yearnow - 39;
                }
                else if (model.Oldclass.FromAge == 32)
                {
                    ageFrom = yearnow - model.Oldclass.FromAge;
                    ageTo = yearnow - 59;
                }
                else if (model.Oldclass.FromAge == 40)
                {
                    ageFrom = yearnow - model.Oldclass.FromAge;
                    ageTo = yearnow - 59;
                }
                else if (model.Oldclass.FromAge == 50)
                {
                    ageFrom = yearnow - model.Oldclass.FromAge;
                    ageTo = yearnow - 99;
                }

                if (_context.Startboats.Where(e => e.RaceId == id).Count() > 0)
                {
                    ViewBag.laststartbahn = _context.Startboats.Last(e => e.RaceId == id).Startslot;
                }

                var allMembers = _context.Members;
                var vStartboats = _context.Startboats.Where(e => e.RaceId == id).OrderBy(e => e.Startslot).ToList();

                ViewBag.startboats = vStartboats;
                ViewBag.startboatmembers = _context.StartboatMembers;
                ViewBag.members = allMembers;

                if (model.Gender == "M" || model.Gender == "W")
                {
                    ViewBag.MemberId = new SelectList(_context.Members.Where(e => e.Gender == model.Gender && e.Birthyear >= ageTo && e.Birthyear <= ageFrom).OrderBy(e => e.LastName).Distinct(), "MemberId", "FullNameClub");
                }
                else
                {
                    ViewBag.MemberId = new SelectList(_context.Members.Where(e => e.Birthyear >= ageTo && e.Birthyear <= ageFrom).OrderBy(e => e.LastName).Distinct(), "MemberId", "FullNameClub");
                }

                ViewBag.ClubId = new SelectList(_context.Clubs.OrderBy(e => e.Name), "ClubId", "Name");
                ViewBag.StartboatstatusId = new SelectList(_context.Startboatstati, "StartboatstatusId", "Name");


                return View(model);
            }
            else
            {
                return View();
            }
        }
    }
}