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
    public class RaceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RaceController(ApplicationDbContext context)
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
            var rid = 0;

            if (_context.Regattas.Where(e => e.Choosen == true).Any())
            {
                rid = _context.Regattas.Where(e => e.Choosen == true).FirstOrDefault().RegattaId;
            }

            ViewBag.regattachosen = rid;

            if (rid != 0)
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

        public IActionResult Edit(int id)
        {
            ViewBag.BoatclassId = new SelectList(_context.Boatclasses.OrderBy(e => e.Name), "BoatclassId", "Name");
            ViewBag.RaceclassId = new SelectList(_context.Raceclasses, "RaceclassId", "Name");
            ViewBag.OldclassId = new SelectList(_context.Oldclasses.OrderBy(e => e.FromAge), "OldclassId", "Name");
            ViewBag.RacestatusId = new SelectList(_context.Racestati, "RacestatusId", "Name");
            ViewBag.ClubId = new SelectList(_context.Clubs, "ClubId", "Name");
            ViewBag.MemberId = new SelectList(_context.Members, "MemberId", "LastName, FirstName");

            var model = _context.Races.Include(e => e.Raceclass).Include(e => e.Boatclass).Include(e => e.Oldclass).FirstOrDefault(e => e.RaceId == id);

            return View(model);
        }

        public IActionResult Update(Race race)
        {
            if (race != null)
            {
                _context.Entry(race).State = EntityState.Modified;
                _context.SaveChanges();
            }

            return RedirectToAction("Details", "Race", new { id = race.RaceId });
        }

        public IActionResult Delete(int id)
        {
            var original = _context.Races.FirstOrDefault(e => e.RaceId == id);
            if (original != null)
            {
                _context.Races.Remove(original);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult AddStartboat(int id, int seat1, int seat2, int seat3, int seat4, int seat5, int seat6, int seat7, int seat8, int startbahn, int ClubId, int seatnumber, int StartboatstatusId)
        {
            _context.Races.Include(e => e.Startboats).FirstOrDefault(e => e.RaceId == id).Startboats.Add(new Startboat { Startslot = startbahn, ClubId = ClubId, StartboatstatusId = StartboatstatusId });
            _context.SaveChanges();

            if (seatnumber == 1)
            {
                _context.Startboats.Include(e => e.StartboatMembers).Last().StartboatMembers.Add(new StartboatMember { MemberId = seat1, SeatNumber = 1, StartboatId = _context.Startboats.Max(i => i.StartboatId) });
                _context.SaveChanges();
            }
            else if (seatnumber == 2)
            {
                _context.Startboats.Include(e => e.StartboatMembers).Last().StartboatMembers.Add(new StartboatMember { MemberId = seat1, SeatNumber = 1, StartboatId = _context.Startboats.Max(i => i.StartboatId) });
                _context.Startboats.Include(e => e.StartboatMembers).Last().StartboatMembers.Add(new StartboatMember { MemberId = seat2, SeatNumber = 2, StartboatId = _context.Startboats.Max(i => i.StartboatId) });
                _context.SaveChanges();
            }
            else if (seatnumber == 4)
            {
                _context.Startboats.Include(e => e.StartboatMembers).Last().StartboatMembers.Add(new StartboatMember { MemberId = seat1, SeatNumber = 1, StartboatId = _context.Startboats.Max(i => i.StartboatId) });
                _context.Startboats.Include(e => e.StartboatMembers).Last().StartboatMembers.Add(new StartboatMember { MemberId = seat2, SeatNumber = 2, StartboatId = _context.Startboats.Max(i => i.StartboatId) });
                _context.Startboats.Include(e => e.StartboatMembers).Last().StartboatMembers.Add(new StartboatMember { MemberId = seat3, SeatNumber = 3, StartboatId = _context.Startboats.Max(i => i.StartboatId) });
                _context.Startboats.Include(e => e.StartboatMembers).Last().StartboatMembers.Add(new StartboatMember { MemberId = seat4, SeatNumber = 4, StartboatId = _context.Startboats.Max(i => i.StartboatId) });
                _context.SaveChanges();
            }
            else if (seatnumber == 8)
            {
                _context.Startboats.Include(e => e.StartboatMembers).Last().StartboatMembers.Add(new StartboatMember { MemberId = seat1, SeatNumber = 1, StartboatId = _context.Startboats.Max(i => i.StartboatId) });
                _context.Startboats.Include(e => e.StartboatMembers).Last().StartboatMembers.Add(new StartboatMember { MemberId = seat2, SeatNumber = 2, StartboatId = _context.Startboats.Max(i => i.StartboatId) });
                _context.Startboats.Include(e => e.StartboatMembers).Last().StartboatMembers.Add(new StartboatMember { MemberId = seat3, SeatNumber = 3, StartboatId = _context.Startboats.Max(i => i.StartboatId) });
                _context.Startboats.Include(e => e.StartboatMembers).Last().StartboatMembers.Add(new StartboatMember { MemberId = seat4, SeatNumber = 4, StartboatId = _context.Startboats.Max(i => i.StartboatId) });
                _context.Startboats.Include(e => e.StartboatMembers).Last().StartboatMembers.Add(new StartboatMember { MemberId = seat5, SeatNumber = 5, StartboatId = _context.Startboats.Max(i => i.StartboatId) });
                _context.Startboats.Include(e => e.StartboatMembers).Last().StartboatMembers.Add(new StartboatMember { MemberId = seat6, SeatNumber = 6, StartboatId = _context.Startboats.Max(i => i.StartboatId) });
                _context.Startboats.Include(e => e.StartboatMembers).Last().StartboatMembers.Add(new StartboatMember { MemberId = seat7, SeatNumber = 7, StartboatId = _context.Startboats.Max(i => i.StartboatId) });
                _context.Startboats.Include(e => e.StartboatMembers).Last().StartboatMembers.Add(new StartboatMember { MemberId = seat8, SeatNumber = 8, StartboatId = _context.Startboats.Max(i => i.StartboatId) });
                _context.SaveChanges();
            }

            return RedirectToAction("Details", "Race", new { id = id });
        }

        public IActionResult DeleteStartboat(int id)
        {
            var original = _context.Startboats.FirstOrDefault(e => e.StartboatId == id);
            if (original != null)
            {
                _context.Startboats.Remove(original);
                _context.SaveChanges();
            }
            return RedirectToAction("Details", "Race", new { id = original.RaceId });
        }
    }
}