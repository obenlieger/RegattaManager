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
    public class RaceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RaceController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string orderby)
        {
            var rid = 0;

            if(_context.Regattas.Where(e => e.Choosen == true).Any())
            { 
                rid = _context.Regattas.Where(e => e.Choosen == true).FirstOrDefault().RegattaId;
            }

            ViewBag.regattachosen = rid;

            if (rid != 0)
            {
                IEnumerable<Race> model = _context.Races.Include(e => e.Boatclass).Include(e => e.Oldclass).Include(e => e.Raceclass).Include(e => e.Regatta).Include(e => e.Racestatus).Include(e => e.Startboats).Where(e => e.RegattaId == rid && e.RacestatusId != 1002 && e.RacestatusId != 1004 && e.RacestatusId != 3 && e.RacestatusId != 1006).OrderBy(e => e.Starttime).ToList();       
                var approveable = _context.Races.Include(e => e.Boatclass).Include(e => e.Oldclass).Include(e => e.Raceclass).Include(e => e.Regatta).Include(e => e.Racestatus).Include(e => e.Startboats).Where(e => e.RegattaId == rid && e.RacestatusId == 1002).OrderBy(e => e.Starttime).ToList();
                var raceable = _context.Races.Include(e => e.Boatclass).Include(e => e.Oldclass).Include(e => e.Raceclass).Include(e => e.Regatta).Include(e => e.Racestatus).Include(e => e.Startboats).Where(e => e.RegattaId == rid && e.RacestatusId == 1004).OrderBy(e => e.Starttime).ToList();
                var finished = _context.Races.Include(e => e.Boatclass).Include(e => e.Oldclass).Include(e => e.Raceclass).Include(e => e.Regatta).Include(e => e.Racestatus).Include(e => e.Startboats).Where(e => e.RegattaId == rid && e.RacestatusId == 3).OrderBy(e => e.Starttime).ToList();
                var notenough = _context.Races.Include(e => e.Boatclass).Include(e => e.Oldclass).Include(e => e.Raceclass).Include(e => e.Regatta).Include(e => e.Racestatus).Include(e => e.Startboats).Where(e => e.RegattaId == rid && e.RacestatusId == 1006).OrderBy(e => e.Starttime).ToList();
                var startboats = _context.Startboats.ToList();                                          

                if (orderby == "RaceCode")
                {
                    model = _context.Races.Include(e => e.Boatclass).Include(e => e.Oldclass).Include(e => e.Raceclass).Include(e => e.Regatta).Include(e => e.Racestatus).Include(e => e.Startboats).Where(e => e.RegattaId == rid && e.RacestatusId != 1002 && e.RacestatusId != 1004 && e.RacestatusId != 3 && e.RacestatusId != 1006).OrderBy(e => e.RaceCode);
                }
                else if(orderby == "Racestatus")
                {
                    model = _context.Races.Include(e => e.Boatclass).Include(e => e.Oldclass).Include(e => e.Raceclass).Include(e => e.Regatta).Include(e => e.Racestatus).Include(e => e.Startboats).Where(e => e.RegattaId == rid && e.RacestatusId != 1002 && e.RacestatusId != 1004 && e.RacestatusId != 3 && e.RacestatusId != 1006).OrderBy(e => e.RacestatusId);
                }
                else if(orderby == "Starttime")
                {
                    model = _context.Races.Include(e => e.Boatclass).Include(e => e.Oldclass).Include(e => e.Raceclass).Include(e => e.Regatta).Include(e => e.Racestatus).Include(e => e.Startboats).Where(e => e.RegattaId == rid && e.RacestatusId != 1002 && e.RacestatusId != 1004 && e.RacestatusId != 3 && e.RacestatusId != 1006).OrderBy(e => e.Starttime);
                }

                ViewBag.approveable = approveable;
                ViewBag.raceable = raceable;
                ViewBag.finished = finished;
                ViewBag.notenough = notenough;
                ViewBag.startboats = startboats;
                ViewBag.rid = rid;

                var regattaClubs = _context.RegattaClubs.Where(e => e.RegattaId == rid);
                ViewBag.clubs = _context.Clubs.Where(e => regattaClubs.Select(i => i.ClubId).Contains(e.ClubId)).ToList();

                return View(model);
            }
            else
            {
                return View();
            }          
        }

        public IActionResult Details(int id, bool doppelt, bool allAvailable, string filterclub)
        {
            var rid = 0;

            if (_context.Regattas.Where(e => e.Choosen == true).Any())
            {
                rid = _context.Regattas.Where(e => e.Choosen == true).FirstOrDefault().RegattaId;
            }

            ViewBag.regattachosen = rid;

            if (rid != 0)
            {
                var model = _context.Races.Include(e => e.Boatclass).Include(e => e.Raceclass).Include(e => e.Oldclass).FirstOrDefault(e => e.RaceId == id);

                int yearnow = DateTime.Now.Year;
                int ageFrom = 0;
                int ageTo = yearnow - model.Oldclass.ToAge;

                if (allAvailable == true)
                {
                    ageFrom = getAgeFrom(model.Oldclass.FromAge, true);
                    ageTo = getAgeTo(model.Oldclass.FromAge, model.Oldclass.ToAge, true);
                }
                else
                {
                    ageFrom = getAgeFrom(model.Oldclass.FromAge, false);
                    ageTo = getAgeTo(model.Oldclass.FromAge, model.Oldclass.ToAge, false);
                }

                var allMembers = _context.Members.Include(e => e.Club).ToList();
                var vStartboats = _context.Startboats.Include(e => e.Startboatstatus).Include(e => e.Club).Where(e => e.RaceId == id).OrderBy(e => e.Startslot).ToList();

                if(model.RacestatusId == 1002 || model.RacestatusId == 3)
                {
                    vStartboats = vStartboats.OrderBy(e => e.Placement).ToList();
                }

                var sbMembers = _context.StartboatMembers.Include(e => e.Member).Where(e => e.Startboat.RaceId == id).Select(e => e.MemberId).ToList();
                var sbStandbys = _context.StartboatStandbys.Include(e => e.Member).Where(e => e.Startboat.RaceId == id).Select(e => e.MemberId).ToList();
                var availMembers = _context.Members.Include(e => e.Club).Where(e => !sbMembers.Contains(e.MemberId));
                allMembers = _context.Members.Include(e => e.Club).ToList();

                if (model.Gender == "M" || model.Gender == "W")
                {
                    var memberlist1 = new SelectList(availMembers.Where(e => e.Gender == model.Gender && e.Birthyear >= ageTo && e.Birthyear <= ageFrom).OrderBy(e => e.LastName).Distinct(), "MemberId", "FullName");
                    if (memberlist1.Count() == 0)
                    {
                        ViewBag.MemberCount = 0;
                    }
                    ViewBag.MemberId = memberlist1;
                }
                else
                {
                    var memberlist2 = new SelectList(availMembers.Where(e => e.Birthyear >= ageTo && e.Birthyear <= ageFrom).OrderBy(e => e.LastName).Distinct(), "MemberId", "FullName");
                    if (memberlist2.Count() == 0)
                    {
                        ViewBag.MemberCount = 0;
                    }
                    ViewBag.MemberId = memberlist2;
                }

                if (_context.Startboats.Where(e => e.RaceId == id).Count() > 0)
                {
                    ViewBag.laststartbahn = _context.Startboats.OrderBy(e => e.Startslot).Last(e => e.RaceId == id).Startslot;
                }

                var regattaClubs = _context.RegattaClubs.Where(e => e.RegattaId == rid);
                var otherRaces = _context.Races.Where(e => e.ReportedRaceId == model.ReportedRaceId && e.RaceId != model.RaceId).OrderBy(e => e.RaceCode).ToList();

                IEnumerable<Club> allClubs = _context.Clubs.Where(e => regattaClubs.Select(i => i.ClubId).Contains(e.ClubId)).OrderBy(e => e.Name);

                ViewBag.startboats = vStartboats;
                ViewBag.startboatmembers = _context.StartboatMembers;
                ViewBag.startboatstandbys = _context.StartboatStandbys;
                ViewBag.members = allMembers;
                ViewBag.AllClubs = allClubs;
                ViewBag.ThisYear = yearnow;
                ViewBag.doppelt = doppelt;
                ViewBag.allAvailable = allAvailable;
                ViewBag.filterclub = filterclub;
                ViewBag.otherRaces = new SelectList(otherRaces, "RaceId", "RaceCode");
                
                ViewBag.clubs = _context.Clubs.Where(e => regattaClubs.Select(i => i.ClubId).Contains(e.ClubId)).ToList();

                if (filterclub != null)
                {
                    ViewBag.ClubId = new SelectList(_context.Clubs.Where(e => e.Name.Contains(filterclub)), "ClubId", "Name");
                }
                else
                {
                    ViewBag.ClubId = new SelectList(allClubs, "ClubId", "Name");
                }                
                ViewBag.StartboatstatusId = new SelectList(_context.Startboatstati, "StartboatstatusId", "Name",6);

                if (model.Gender == "M")
                {
                    ViewBag.genderdesc = "männliche";
                }
                if (model.Gender == "W")
                {
                    ViewBag.genderdesc = "weibliche";
                }
                if (model.Gender == "X")
                {
                    ViewBag.genderdesc = "mixed";
                }

                if(model.RacestatusId == 1002)
                {
                    ViewBag.StarboatStatusId = new SelectList(_context.Startboatstati, "StartboatstatusId", "Name");
                }

                return View(model);
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.BoatclassId = new SelectList(_context.Boatclasses.OrderBy(e => e.Name), "BoatclassId", "Name");
            ViewBag.RaceclassId = new SelectList(_context.Raceclasses, "RaceclassId", "Name");
            ViewBag.OldclassId = new SelectList(_context.Oldclasses.OrderBy(e => e.FromAge), "OldclassId", "Name");
            ViewBag.RacestatusId = new SelectList(_context.Racestati, "RacestatusId", "Name");
            ViewBag.RaceTypId = new SelectList(_context.RaceTyps, "RaceTypId", "Name");
            ViewBag.RegattaId = new SelectList(_context.Regattas, "RegattaId", "Name");

            return View();
        }

        [HttpPost]
        public IActionResult Create(Race race)
        {
            if (race != null)
            {
                _context.Races.Add(race);
                _context.SaveChanges();
            }

            return RedirectToAction("Details", "Race", new { id = race.RaceId });
        }

        public IActionResult Edit(int id)
        {
            var model = _context.Races.Include(e => e.Raceclass).Include(e => e.Boatclass).Include(e => e.Oldclass).FirstOrDefault(e => e.RaceId == id);

            ViewBag.BoatclassId = new SelectList(_context.Boatclasses.OrderBy(e => e.Name), "BoatclassId", "Name", model.BoatclassId);
            ViewBag.RaceclassId = new SelectList(_context.Raceclasses, "RaceclassId", "Name", model.RaceclassId);
            ViewBag.OldclassId = new SelectList(_context.Oldclasses.OrderBy(e => e.FromAge), "OldclassId", "Name", model.OldclassId);
            ViewBag.RacestatusId = new SelectList(_context.Racestati, "RacestatusId", "Name", model.RacestatusId);
            ViewBag.RaceTypId = new SelectList(_context.RaceTyps, "RaceTypId", "Name", model.RaceTypId);
            ViewBag.RacestatusId = new SelectList(_context.Racestati, "RacestatusId", "Name", model.RacestatusId);

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

        public IActionResult PrintView()
        {
            var rid = 0;

            if (_context.Regattas.Where(e => e.Choosen == true).Any())
            {
                rid = _context.Regattas.Where(e => e.Choosen == true).FirstOrDefault().RegattaId;
            }

            if(rid != 0)
            {
                //var model = _context.Races.Include(e => e.Boatclass).Include(e => e.Oldclass).Include(e => e.Raceclass).Include(e => e.Racestatus).Include(e => e.Startboats).Where(e => e.RacestatusId == 1005 && e.Starttime.Day == DateTime.Now.Day).OrderBy(e => e.Starttime).ToList();
                var model = _context.Races.Include(e => e.Boatclass).Include(e => e.Oldclass).Include(e => e.Raceclass).Include(e => e.Racestatus).Include(e => e.Startboats).Where(e => e.RacestatusId == 1005).OrderBy(e => e.Starttime).ToList();

                ViewBag.startboats = _context.Startboats.Include(e => e.Club).OrderBy(e => e.Startslot).ToList();
                ViewBag.startboatmembers = _context.StartboatMembers.ToList();
                ViewBag.startboatstandbys = _context.StartboatStandbys.ToList();
                ViewBag.members = _context.Members.Include(e => e.Club).ToList();
                ViewBag.racedrawrules = _context.RaceDrawRules.Include(e => e.RaceDraw).Include(e => e.RaceTyp).ToList();
                ViewBag.regatta = _context.Regattas.FirstOrDefault(e => e.RegattaId == rid);
                ViewBag.ThisYear = DateTime.Now.Year;

                var regattaClubs = _context.RegattaClubs.Where(e => e.RegattaId == rid);
                ViewBag.clubs = _context.Clubs.Where(e => regattaClubs.Select(i => i.ClubId).Contains(e.ClubId)).ToList();

                return View(model);
            }

            return NotFound();
        }

        public IActionResult PrintResult()
        {
            var rid = 0;

            if (_context.Regattas.Where(e => e.Choosen == true).Any())
            {
                rid = _context.Regattas.Where(e => e.Choosen == true).FirstOrDefault().RegattaId;
            }

            if (rid != 0)
            {
                var model = _context.Races.Include(e => e.Boatclass).Include(e => e.Oldclass).Include(e => e.Raceclass).Include(e => e.Racestatus).Include(e => e.Startboats).Where(e => e.RacestatusId == 3).OrderBy(e => e.Starttime).ToList();

                ViewBag.startboats = _context.Startboats.Include(e => e.Club).Include(e => e.Startboatstatus).Where(e => e.StartboatstatusId == 3).OrderBy(e => e.Placement).ToList();
                ViewBag.disqstartboats = _context.Startboats.Include(e => e.Club).Include(e => e.Startboatstatus).Where(e => e.StartboatstatusId == 4 || e.StartboatstatusId == 5 || e.StartboatstatusId == 7 || e.StartboatstatusId == 8).OrderBy(e => e.Placement).ToList();
                ViewBag.startboatmembers = _context.StartboatMembers.ToList();
                ViewBag.startboatstandbys = _context.StartboatStandbys.ToList();
                ViewBag.members = _context.Members.Include(e => e.Club).ToList();
                ViewBag.racedrawrules = _context.RaceDrawRules.Include(e => e.RaceDraw).Include(e => e.RaceTyp).ToList();
                ViewBag.regatta = _context.Regattas.FirstOrDefault(e => e.RegattaId == rid);
                ViewBag.ThisYear = DateTime.Now.Year;

                var regattaClubs = _context.RegattaClubs.Where(e => e.RegattaId == rid);
                ViewBag.clubs = _context.Clubs.Where(e => regattaClubs.Select(i => i.ClubId).Contains(e.ClubId)).ToList();

                return View(model);
            }
            return NotFound();
        }

        public IActionResult MoveStartboatToRace(int id, int toRaceId)
        {
            var startboat = _context.Startboats.FirstOrDefault(e => e.StartboatId == id);
            var oldraceid = startboat.RaceId;

            if(startboat != null)
            {
                var torace = _context.Races.FirstOrDefault(e => e.RaceId == toRaceId);

                if(torace != null)
                {
                    int startslot = 1;

                    if(_context.Startboats.Where(e => e.RaceId == toRaceId).Any())
                    {
                        startslot = _context.Startboats.Where(e => e.RaceId == toRaceId).OrderBy(e => e.Startslot).Select(e => e.Startslot).Last() + 1;
                    }
                    
                    startboat.RaceId = toRaceId;
                    startboat.Startslot = startslot;

                    _context.Startboats.Update(startboat);
                    _context.SaveChanges();
                }
            }

            return RedirectToAction("Details", "Race", new { id = oldraceid });
        }

        public IActionResult AddStartboat(int id, int seat1, int seat2, int seat3, int seat4, int seat5, int seat6, int seat7, int seat8,
            int standby1, int standby2, int standby3, int standby4, int standby5, int standby6, int standby7, int standby8, 
            bool standbycheck1, bool standbycheck2, bool standbycheck3, bool standbycheck4, bool standbycheck5,
            bool standbycheck6, bool standbycheck7, bool standbycheck8, int startbahn, int ClubId, int seatnumber, int StartboatstatusId)
        {
            var race = _context.Races.FirstOrDefault(e => e.RaceId == id);            
            var reportedraceid = _context.ReportedRaces.FirstOrDefault(e => e.RaceCode.Substring(0, 5) == race.RaceCode.Substring(0, 5)).ReportedRaceId;

            var result = AddReportedStartboat(reportedraceid, seat1, seat2, seat3, seat4, seat5, seat6, seat7, seat8, standby1, standby2, standby3, standby4, standby5, standby6, standby7, standby8, standbycheck1, standbycheck2, standbycheck3, standbycheck4, standbycheck5, standbycheck6, standbycheck7, standbycheck8, ClubId, seatnumber,race.RegattaId);

            if(result == 0)
            {
                var reportedstartboatid = _context.ReportedStartboats.Max(e => e.ReportedStartboatId);
                _context.Races.Include(e => e.Startboats).FirstOrDefault(e => e.RaceId == id).Startboats.Add(new Startboat { Startslot = startbahn, ClubId = ClubId, StartboatstatusId = StartboatstatusId, ReportedStartboatId = reportedstartboatid, RegattaId = race.RegattaId });
                _context.SaveChanges();

                if (seatnumber == 1)
                {
                    _context.StartboatMembers.Add(new StartboatMember { MemberId = seat1, SeatNumber = 1, StartboatId = _context.Startboats.Max(i => i.StartboatId) });
                    _context.SaveChanges();
                }
                else if (seatnumber == 2)
                {
                    _context.StartboatMembers.Add(new StartboatMember { MemberId = seat1, SeatNumber = 1, StartboatId = _context.Startboats.Max(i => i.StartboatId) });
                    _context.StartboatMembers.Add(new StartboatMember { MemberId = seat2, SeatNumber = 2, StartboatId = _context.Startboats.Max(i => i.StartboatId) });
                    _context.SaveChanges();
                }
                else if (seatnumber == 3)
                {
                    _context.StartboatMembers.Add(new StartboatMember { MemberId = seat1, SeatNumber = 1, StartboatId = _context.Startboats.Max(i => i.StartboatId) });
                    _context.StartboatMembers.Add(new StartboatMember { MemberId = seat2, SeatNumber = 2, StartboatId = _context.Startboats.Max(i => i.StartboatId) });
                    _context.StartboatMembers.Add(new StartboatMember { MemberId = seat3, SeatNumber = 3, StartboatId = _context.Startboats.Max(i => i.StartboatId) });
                    _context.SaveChanges();
                }
                else if (seatnumber == 4)
                {
                    _context.StartboatMembers.Add(new StartboatMember { MemberId = seat1, SeatNumber = 1, StartboatId = _context.Startboats.Max(i => i.StartboatId) });
                    _context.StartboatMembers.Add(new StartboatMember { MemberId = seat2, SeatNumber = 2, StartboatId = _context.Startboats.Max(i => i.StartboatId) });
                    _context.StartboatMembers.Add(new StartboatMember { MemberId = seat3, SeatNumber = 3, StartboatId = _context.Startboats.Max(i => i.StartboatId) });
                    _context.StartboatMembers.Add(new StartboatMember { MemberId = seat4, SeatNumber = 4, StartboatId = _context.Startboats.Max(i => i.StartboatId) });
                    _context.SaveChanges();
                }
                else if (seatnumber == 8)
                {
                    _context.StartboatMembers.Add(new StartboatMember { MemberId = seat1, SeatNumber = 1, StartboatId = _context.Startboats.Max(i => i.StartboatId) });
                    _context.StartboatMembers.Add(new StartboatMember { MemberId = seat2, SeatNumber = 2, StartboatId = _context.Startboats.Max(i => i.StartboatId) });
                    _context.StartboatMembers.Add(new StartboatMember { MemberId = seat3, SeatNumber = 3, StartboatId = _context.Startboats.Max(i => i.StartboatId) });
                    _context.StartboatMembers.Add(new StartboatMember { MemberId = seat4, SeatNumber = 4, StartboatId = _context.Startboats.Max(i => i.StartboatId) });
                    _context.StartboatMembers.Add(new StartboatMember { MemberId = seat5, SeatNumber = 5, StartboatId = _context.Startboats.Max(i => i.StartboatId) });
                    _context.StartboatMembers.Add(new StartboatMember { MemberId = seat6, SeatNumber = 6, StartboatId = _context.Startboats.Max(i => i.StartboatId) });
                    _context.StartboatMembers.Add(new StartboatMember { MemberId = seat7, SeatNumber = 7, StartboatId = _context.Startboats.Max(i => i.StartboatId) });
                    _context.StartboatMembers.Add(new StartboatMember { MemberId = seat8, SeatNumber = 8, StartboatId = _context.Startboats.Max(i => i.StartboatId) });
                    _context.SaveChanges();
                }
            }          

            return RedirectToAction("Details", "Race", new { id = id });
        }

        public IActionResult DeleteStartboat(int id)
        {
            var original = _context.Startboats.FirstOrDefault(e => e.StartboatId == id);
            var startboatmembers = _context.StartboatMembers.Where(e => e.StartboatId == id);
            var startboatstandbys = _context.StartboatStandbys.Where(e => e.StartboatId == id);

            foreach(var sbm in startboatmembers)
            {
                _context.StartboatMembers.Remove(sbm);
            }
            foreach (var sbs in startboatstandbys)
            {
                _context.StartboatStandbys.Remove(sbs);
            }
            _context.SaveChanges();

            if (original != null)
            {
                _context.Startboats.Remove(original);
                _context.SaveChanges();
            }
            return RedirectToAction("Details", "Race", new { id = original.RaceId });
        }

        public IActionResult StartSlotUp(int id)
        {
            var original = _context.Startboats.FirstOrDefault(e => e.StartboatId == id);

            if(original != null)
            {
                if(original.Startslot > 1)
                {
                    if (!_context.Startboats.Any(e => e.RaceId == original.RaceId && e.Startslot == original.Startslot - 1))
                    {
                        original.Startslot = original.Startslot - 1;
                        _context.Startboats.Update(original);
                        _context.SaveChanges();
                    }
                    else
                    {
                        var nextsb = _context.Startboats.FirstOrDefault(e => e.RaceId == original.RaceId && e.Startslot == original.Startslot - 1);
                        nextsb.Startslot = nextsb.Startslot + 1;
                        original.Startslot = original.Startslot - 1;
                        _context.Startboats.Update(original);
                        _context.SaveChanges();
                    }
                }                
            }

            return RedirectToAction("Details", "Race", new { id = original.RaceId });
        }

        public IActionResult StartSlotDown(int id)
        {
            var original = _context.Startboats.FirstOrDefault(e => e.StartboatId == id);

            if (original != null)
            {
                if(!_context.Startboats.Any(e => e.RaceId == original.RaceId && e.Startslot == original.Startslot + 1))
                {
                    original.Startslot = original.Startslot + 1;
                    _context.Startboats.Update(original);
                    _context.SaveChanges();
                }          
                else
                {
                    var nextsb = _context.Startboats.FirstOrDefault(e => e.RaceId == original.RaceId && e.Startslot == original.Startslot + 1);
                    nextsb.Startslot = nextsb.Startslot - 1;
                    original.Startslot = original.Startslot + 1;
                    _context.Startboats.Update(original);
                    _context.SaveChanges();
                }
            }

            return RedirectToAction("Details", "Race", new { id = original.RaceId });
        }

        public IActionResult PlacementUp(int id)
        {
            var original = _context.Startboats.FirstOrDefault(e => e.StartboatId == id);

            if (original != null)
            {
                if (original.Placement > 1)
                {
                    if (!_context.Startboats.Any(e => e.RaceId == original.RaceId && e.Placement == original.Placement - 1))
                    {
                        original.Placement = original.Placement - 1;
                        _context.Startboats.Update(original);
                        _context.SaveChanges();
                    }
                    else
                    {
                        var nextsb = _context.Startboats.FirstOrDefault(e => e.RaceId == original.RaceId && e.Placement == original.Placement - 1);
                        nextsb.Placement = nextsb.Placement + 1;
                        original.Placement = original.Placement - 1;
                        _context.Startboats.Update(original);
                        _context.SaveChanges();
                    }
                }
            }

            return RedirectToAction("Details", "Race", new { id = original.RaceId });
        }

        public IActionResult PlacementDown(int id)
        {
            var original = _context.Startboats.FirstOrDefault(e => e.StartboatId == id);

            if (original != null)
            {
                if (!_context.Startboats.Any(e => e.RaceId == original.RaceId && e.Placement == original.Placement + 1))
                {
                    original.Placement = original.Placement + 1;
                    _context.Startboats.Update(original);
                    _context.SaveChanges();
                }
                else
                {
                    var nextsb = _context.Startboats.FirstOrDefault(e => e.RaceId == original.RaceId && e.Placement == original.Placement + 1);
                    nextsb.Placement = nextsb.Placement - 1;
                    original.Placement = original.Placement + 1;
                    _context.Startboats.Update(original);
                    _context.SaveChanges();
                }
            }

            return RedirectToAction("Details", "Race", new { id = original.RaceId });
        }

        public IActionResult Approve(int id)
        {
            var model = _context.Races.FirstOrDefault(e => e.RaceId == id);

            if(model != null)
            {
                model.RacestatusId = 3;
                _context.Races.Update(model);
                _context.SaveChanges();
            }

            bool drawresult = TryDrawRaces(id);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult DisApprove(int id)
        {
            var model = _context.Races.FirstOrDefault(e => e.RaceId == id);

            if (model != null)
            {
                model.RacestatusId = 1002;
                _context.Races.Update(model);
                _context.SaveChanges();
            }

            return RedirectToAction("Details", new { id = id });
        }

        public IActionResult ApproveDraw(int id)
        {
            var model = _context.Races.FirstOrDefault(e => e.RaceId == id);

            if (model != null)
            {
                model.RacestatusId = 1005;
                _context.Races.Update(model);
                _context.SaveChanges();
            }

            bool drawresult = TryDrawRaces(id);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult EditStartboatStatus(int id, int statusid)
        {
            var startboat = _context.Startboats.FirstOrDefault(e => e.StartboatId == id);

            if(startboat != null)
            {
                startboat.StartboatstatusId = statusid;

                if(statusid == 4 || statusid == 7 || statusid == 8)
                {
                    startboat.Placement = -1;
                }

                _context.Startboats.Update(startboat);
                _context.SaveChanges();
            }

            return RedirectToAction("Details", new { id = startboat.RaceId });
        }

        [HttpGet]
        public ActionResult EditStartboat(int id, bool allAvailable)
        {
            var startboat = _context.Startboats.Include(e => e.StartboatMembers).Include(e => e.StartboatStandbys).FirstOrDefault(e => e.StartboatId == id);
            var model = _context.Races.Include(e => e.Boatclass).Include(e => e.Raceclass).Include(e => e.Oldclass).FirstOrDefault(e => e.RaceId == startboat.RaceId);

            int yearnow = DateTime.Now.Year;
            int ageFrom = 0;
            int ageTo = yearnow - model.Oldclass.ToAge;

            if (allAvailable == true)
            {
                ageFrom = getAgeFrom(model.Oldclass.FromAge, true);
                ageTo = getAgeTo(model.Oldclass.FromAge, model.Oldclass.ToAge, true);
            }
            else
            {
                ageFrom = getAgeFrom(model.Oldclass.FromAge, false);
                ageTo = getAgeTo(model.Oldclass.FromAge, model.Oldclass.ToAge, false);
            }

            var clubid = startboat.ClubId;
            var sbMembers = _context.StartboatMembers.Include(e => e.Member).Where(e => e.Startboat.RaceId == startboat.RaceId).Select(e => e.MemberId).ToList();
            var sbStandbys = _context.StartboatStandbys.Include(e => e.Member).Where(e => e.Startboat.RaceId == startboat.RaceId).Select(e => e.MemberId).ToList();
            var allMembers = _context.Members.Include(e => e.Club).OrderBy(e => e.LastName);
            var vStartboats = _context.Startboats.Where(e => e.RaceId == startboat.RaceId).ToList();
            var editSBMember = _context.StartboatMembers.Where(e => e.StartboatId == startboat.StartboatId).OrderBy(e => e.SeatNumber);
            var editSBStandby = _context.StartboatStandbys.Where(e => e.StartboatId == startboat.StartboatId).OrderBy(e => e.Standbynumber);
            IQueryable<Member> tempmemberlist = _context.Members;
            IQueryable<Member> tempstandbylist = _context.Members;

            int i = 0;
            Member[] member = new Member[model.Boatclass.Seats];
            SelectList[] memberlist = new SelectList[model.Boatclass.Seats];
            Member[] standby = new Member[model.Boatclass.Seats];
            SelectList[] standbylist = new SelectList[model.Boatclass.Seats];
            string listitem = "";
            string sbscheck = "";
            string oldmemberid = "";
            string oldstandbyid = "";

            foreach (var esbm in editSBMember)
            {
                member[i] = _context.Members.FirstOrDefault(e => e.MemberId == esbm.MemberId);
                oldmemberid = "oldmemberid" + i;
                ViewData[oldmemberid] = member[i].MemberId;
                tempmemberlist = _context.Members.Where(e => (!sbMembers.Contains(e.MemberId)) || e.MemberId == esbm.MemberId);

                if (model.Gender == "M" || model.Gender == "W")
                {
                    tempmemberlist = tempmemberlist.Where(e => e.Gender == model.Gender && e.Birthyear >= ageTo && e.Birthyear <= ageFrom).OrderBy(e => e.LastName).Distinct();
                    memberlist[i] = new SelectList(tempmemberlist, "MemberId", "FullName", member[i].MemberId);
                }
                else
                {
                    tempmemberlist = tempmemberlist.Where(e => e.Birthyear >= ageTo && e.Birthyear <= ageFrom).OrderBy(e => e.LastName).Distinct();
                    memberlist[i] = new SelectList(tempmemberlist, "MemberId", "FullName", member[i].MemberId);
                }

                listitem = "memberlist" + i;
                ViewData[listitem] = memberlist[i];

                if (editSBStandby.Any(e => e.Standbynumber == i + 1))
                {
                    var sbs = editSBStandby.FirstOrDefault(e => e.Standbynumber == i + 1);
                    standby[i] = _context.Members.FirstOrDefault(e => e.MemberId == sbs.MemberId);
                    tempstandbylist = _context.Members.Where(e => (!sbMembers.Contains(e.MemberId)) || e.MemberId == sbs.MemberId);

                    if (model.Gender == "M" || model.Gender == "W")
                    {
                        tempstandbylist = tempstandbylist.Where(e => e.Gender == model.Gender && e.Birthyear >= ageTo && e.Birthyear <= ageFrom).OrderBy(e => e.LastName).Distinct();
                        standbylist[i] = new SelectList(tempstandbylist, "MemberId", "FullName", standby[i].MemberId);
                    }
                    else
                    {
                        tempstandbylist = tempstandbylist.Where(e => e.Birthyear >= ageTo && e.Birthyear <= ageFrom).OrderBy(e => e.LastName).Distinct();
                        standbylist[i] = new SelectList(tempstandbylist, "MemberId", "FullName", standby[i].MemberId);
                    }

                    sbscheck = "sbscheck" + i;
                    ViewData[sbscheck] = "checked";
                    listitem = "standbylist" + i;
                    ViewData[listitem] = standbylist[i];
                    oldstandbyid = "oldstandbyid" + i;
                    ViewData[oldstandbyid] = standby[i].MemberId;
                }
                else
                {
                    tempstandbylist = _context.Members.Where(e => !sbMembers.Contains(e.MemberId));

                    if (model.Gender == "M" || model.Gender == "W")
                    {
                        tempstandbylist = tempstandbylist.Where(e => e.Gender == model.Gender && e.Birthyear >= ageTo && e.Birthyear <= ageFrom).OrderBy(e => e.LastName).Distinct();
                        standbylist[i] = new SelectList(tempstandbylist, "MemberId", "FullName");
                    }
                    else
                    {
                        tempstandbylist = tempstandbylist.Where(e => e.Birthyear >= ageTo && e.Birthyear <= ageFrom).OrderBy(e => e.LastName).Distinct();
                        standbylist[i] = new SelectList(tempstandbylist, "MemberId", "FullName");
                    }

                    listitem = "standbylist" + i;
                    ViewData[listitem] = standbylist[i];
                    sbscheck = "sbscheck" + i;
                    ViewData[sbscheck] = "";
                }

                i++;
            }

            IEnumerable<Club> allClubs = _context.Clubs;

            ViewBag.startboats = vStartboats;
            ViewBag.startboatmembers = _context.StartboatMembers;
            ViewBag.startboatstandbys = _context.StartboatStandbys;
            ViewBag.members = allMembers;
            ViewBag.AllClubs = allClubs;
            ViewBag.ThisYear = yearnow;
            ViewBag.Club = _context.Clubs.FirstOrDefault(e => e.ClubId == startboat.ClubId);
            ViewBag.Startboat = startboat;
            ViewBag.StartboatId = startboat.StartboatId;
            ViewBag.Startslot = startboat.Startslot;

            if (model.Gender == "M")
            {
                ViewBag.genderdesc = "männliche";
            }
            if (model.Gender == "W")
            {
                ViewBag.genderdesc = "weibliche";
            }
            if (model.Gender == "X")
            {
                ViewBag.genderdesc = "mixed";
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult EditStartboat(int id, int seat1, int seat2, int seat3, int seat4, int seat5, int seat6, int seat7, int seat8, int standby1, int standby2, int standby3, int standby4,
            int standby5, int standby6, int standby7, int standby8, bool standbycheck1, bool standbycheck2, bool standbycheck3, bool standbycheck4, bool standbycheck5,
            bool standbycheck6, bool standbycheck7, bool standbycheck8, int oldmemberid1, int oldmemberid2, int oldmemberid3, int oldmemberid4, int oldmemberid5, int oldmemberid6,
            int oldmemberid7, int oldmemberid8, int oldstandbyid1, int oldstandbyid2, int oldstandbyid3, int oldstandbyid4, int oldstandbyid5, int oldstandbyid6, int oldstandbyid7,
            int oldstandbyid8, int clubid, int seatnumber, int Startslot)
        {            
            var startboat = _context.Startboats.FirstOrDefault(e => e.StartboatId == id);
            var startboatmembers = _context.StartboatMembers.Where(e => e.StartboatId == id);
            var startboatstandbys = _context.StartboatStandbys.Where(e => e.StartboatId == id);
            var race = _context.Races.FirstOrDefault(e => e.RaceId == startboat.RaceId);
            var rid = race.RegattaId;

            List<int> seats = new List<int>();

            bool isDouble = false;

            if (seatnumber == 1)
            {
                if (seat1 == 0)
                {
                    return RedirectToAction("Details", "Race", new { id = startboat.RaceId });
                }
                seats.Add(seat1);

                if (standbycheck1 == true)
                {
                    if (standby1 == 0)
                    {
                        return RedirectToAction("Details", "Race", new { id = startboat.RaceId });
                    }
                    seats.Add(standby1);
                }

                if (seats.GroupBy(s => s).SelectMany(grp => grp.Skip(1)).Count() >= 1)
                {
                    isDouble = true;
                    ViewBag.doppelt = true;
                }

                if (isDouble == false)
                {
                    EditSeat(1, seat1, standbycheck1, 1, standby1, oldmemberid1, oldstandbyid1, startboat, startboatmembers, startboatstandbys);
                }
            }
            else if (seatnumber == 2)
            {
                if (seat1 == 0 || seat2 == 0)
                {
                    return RedirectToAction("Details", "Race", new { id = startboat.RaceId });
                }
                seats.Add(seat1);
                seats.Add(seat2);

                if (standbycheck1 == true)
                {
                    if (standby1 == 0)
                    {
                        return RedirectToAction("Details", "Race", new { id = startboat.RaceId });
                    }
                    seats.Add(standby1);
                }
                if (standbycheck2 == true)
                {
                    if (standby2 == 0)
                    {
                        return RedirectToAction("Details", "Race", new { id = startboat.RaceId });
                    }
                    seats.Add(standby2);
                }

                if (seats.GroupBy(s => s).SelectMany(grp => grp.Skip(1)).Count() >= 1)
                {
                    isDouble = true;
                    ViewBag.doppelt = true;
                }

                if (isDouble == false)
                {
                    EditSeat(1, seat1, standbycheck1, 1, standby1, oldmemberid1, oldstandbyid1, startboat, startboatmembers, startboatstandbys);
                    EditSeat(2, seat2, standbycheck2, 2, standby2, oldmemberid2, oldstandbyid2, startboat, startboatmembers, startboatstandbys);
                }
            }
            else if (seatnumber == 4)
            {
                if (seat1 == 0 || seat2 == 0 || seat3 == 0 || seat4 == 0)
                {
                    return RedirectToAction("Details", "Race", new { id = startboat.RaceId });
                }
                seats.Add(seat1);
                seats.Add(seat2);
                seats.Add(seat3);
                seats.Add(seat4);

                if (standbycheck1 == true)
                {
                    if (standby1 == 0)
                    {
                        return RedirectToAction("Details", "Race", new { id = startboat.RaceId });
                    }
                    seats.Add(standby1);
                }
                if (standbycheck2 == true)
                {
                    if (standby2 == 0)
                    {
                        return RedirectToAction("Details", "Race", new { id = startboat.RaceId });
                    }
                    seats.Add(standby2);
                }
                if (standbycheck3 == true)
                {
                    if (standby3 == 0)
                    {
                        return RedirectToAction("Details", "Race", new { id = startboat.RaceId });
                    }
                    seats.Add(standby3);
                }
                if (standbycheck4 == true)
                {
                    if (standby4 == 0)
                    {
                        return RedirectToAction("Details", "Race", new { id = startboat.RaceId });
                    }
                    seats.Add(standby4);
                }

                if (seats.GroupBy(s => s).SelectMany(grp => grp.Skip(1)).Count() >= 1)
                {
                    isDouble = true;
                    ViewBag.doppelt = true;
                }

                if (isDouble == false)
                {
                    EditSeat(1, seat1, standbycheck1, 1, standby1, oldmemberid1, oldstandbyid1, startboat, startboatmembers, startboatstandbys);
                    EditSeat(2, seat2, standbycheck2, 2, standby2, oldmemberid2, oldstandbyid2, startboat, startboatmembers, startboatstandbys);
                    EditSeat(3, seat3, standbycheck3, 3, standby3, oldmemberid3, oldstandbyid3, startboat, startboatmembers, startboatstandbys);
                    EditSeat(4, seat4, standbycheck4, 4, standby4, oldmemberid4, oldstandbyid4, startboat, startboatmembers, startboatstandbys);
                }
            }
            else if (seatnumber == 8)
            {
                if (seat1 == 0 || seat2 == 0 || seat3 == 0 || seat4 == 0 || seat5 == 0 || seat6 == 0 || seat7 == 0 || seat8 == 0)
                {
                    return RedirectToAction("Details", "Race", new { id = startboat.RaceId });
                }
                seats.Add(seat1);
                seats.Add(seat2);
                seats.Add(seat3);
                seats.Add(seat4);
                seats.Add(seat5);
                seats.Add(seat6);
                seats.Add(seat7);
                seats.Add(seat8);

                if (standbycheck1 == true)
                {
                    if (standby1 == 0)
                    {
                        return RedirectToAction("Details", "Race", new { id = startboat.RaceId });
                    }
                    seats.Add(standby1);
                }
                if (standbycheck2 == true)
                {
                    if (standby2 == 0)
                    {
                        return RedirectToAction("Details", "Race", new { id = startboat.RaceId });
                    }
                    seats.Add(standby2);
                }
                if (standbycheck3 == true)
                {
                    if (standby3 == 0)
                    {
                        return RedirectToAction("Details", "Race", new { id = startboat.RaceId });
                    }
                    seats.Add(standby3);
                }
                if (standbycheck4 == true)
                {
                    if (standby4 == 0)
                    {
                        return RedirectToAction("Details", "Race", new { id = startboat.RaceId });
                    }
                    seats.Add(standby4);
                }
                if (standbycheck5 == true)
                {
                    if (standby5 == 0)
                    {
                        return RedirectToAction("Details", "Race", new { id = startboat.RaceId });
                    }
                    seats.Add(standby5);
                }
                if (standbycheck6 == true)
                {
                    if (standby6 == 0)
                    {
                        return RedirectToAction("Details", "Race", new { id = startboat.RaceId });
                    }
                    seats.Add(standby6);
                }
                if (standbycheck7 == true)
                {
                    if (standby7 == 0)
                    {
                        return RedirectToAction("Details", "Race", new { id = startboat.RaceId });
                    }
                    seats.Add(standby7);
                }
                if (standbycheck8 == true)
                {
                    if (standby8 == 0)
                    {
                        return RedirectToAction("Details", "Race", new { id = startboat.RaceId });
                    }
                    seats.Add(standby8);
                }

                if (seats.GroupBy(s => s).SelectMany(grp => grp.Skip(1)).Count() >= 1)
                {
                    isDouble = true;
                    ViewBag.doppelt = true;
                }

                if (isDouble == false)
                {
                    EditSeat(1, seat1, standbycheck1, 1, standby1, oldmemberid1, oldstandbyid1, startboat, startboatmembers, startboatstandbys);
                    EditSeat(2, seat2, standbycheck2, 2, standby2, oldmemberid2, oldstandbyid2, startboat, startboatmembers, startboatstandbys);
                    EditSeat(3, seat3, standbycheck3, 3, standby3, oldmemberid3, oldstandbyid3, startboat, startboatmembers, startboatstandbys);
                    EditSeat(4, seat4, standbycheck4, 4, standby4, oldmemberid4, oldstandbyid4, startboat, startboatmembers, startboatstandbys);
                    EditSeat(5, seat5, standbycheck5, 5, standby5, oldmemberid5, oldstandbyid5, startboat, startboatmembers, startboatstandbys);
                    EditSeat(6, seat6, standbycheck6, 6, standby6, oldmemberid6, oldstandbyid6, startboat, startboatmembers, startboatstandbys);
                    EditSeat(7, seat7, standbycheck7, 7, standby7, oldmemberid7, oldstandbyid7, startboat, startboatmembers, startboatstandbys);
                    EditSeat(8, seat8, standbycheck8, 8, standby8, oldmemberid8, oldstandbyid8, startboat, startboatmembers, startboatstandbys);
                }
            }

            var oldstartboat = _context.Startboats.FirstOrDefault(e => e.StartboatId == startboat.StartboatId);

            if(oldstartboat.Startslot != Startslot)
            {
                oldstartboat.Startslot = Startslot;
                _context.Startboats.Update(oldstartboat);
                _context.SaveChanges();
            }

            return RedirectToAction("Details", "Race", new { id = startboat.RaceId });
        }

        public IActionResult TimeUp(int id)
        {
            var race = _context.Races.FirstOrDefault(e => e.RaceId == id);

            List<Race> racebefore = _context.Races.Where(e => e.Starttime < race.Starttime).OrderBy(e => e.Starttime).ToList();

            DateTime beforetime = racebefore.Last().Starttime;
            DateTime origtime = race.Starttime;
            int beforeraceid = racebefore.Last().RaceId;

            race.Starttime = beforetime;

            var beforerace = _context.Races.FirstOrDefault(e => e.RaceId == beforeraceid);

            beforerace.Starttime = origtime;

            if(race != null && beforerace != null)
            {
                _context.Update(race);
                _context.Update(beforerace);
                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Race");
        }

        public IActionResult TimeDown(int id)
        {
            var race = _context.Races.FirstOrDefault(e => e.RaceId == id);

            List<Race> raceafter = _context.Races.Where(e => e.Starttime > race.Starttime).OrderByDescending(e => e.Starttime).ToList();

            DateTime aftertime = raceafter.Last().Starttime;
            DateTime origtime = race.Starttime;
            int afterraceid = raceafter.Last().RaceId;

            race.Starttime = aftertime;

            var afterrace = _context.Races.FirstOrDefault(e => e.RaceId == afterraceid);

            afterrace.Starttime = origtime;

            if(race != null && afterrace != null)
            {
                _context.Update(race);
                _context.Update(afterrace);
                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Race");
        }

        private void EditSeat(int seatnumber, int seatmemberid, bool standbycheck, int standbynumber, int standbymemberid, int oldmemberid, int oldstandbyid
        , Startboat startboat, IQueryable<StartboatMember> startboatmembers, IQueryable<StartboatStandby> startboatstandbys)
        {
            if (startboatmembers.Any(e => e.SeatNumber == seatnumber) && oldmemberid != seatmemberid)
            {
                var sbm = _context.StartboatMembers.FirstOrDefault(e => e.MemberId == oldmemberid && e.StartboatId == startboat.StartboatId && e.SeatNumber == seatnumber);
                if (sbm != null)
                {
                    _context.StartboatMembers.Remove(sbm);
                    _context.StartboatMembers.Add(new StartboatMember { MemberId = seatmemberid, SeatNumber = seatnumber, StartboatId = startboat.StartboatId });
                }
            }

            if (standbycheck == true)
            {
                if (startboatstandbys.Any(e => e.Standbynumber == standbynumber) && oldstandbyid != standbymemberid)
                {
                    var sbs = _context.StartboatStandbys.FirstOrDefault(e => e.Standbynumber == standbynumber && e.MemberId == oldstandbyid && e.StartboatId == startboat.StartboatId);

                    if (sbs != null)
                    {
                        _context.StartboatStandbys.Remove(sbs);
                        _context.StartboatStandbys.Add(new StartboatStandby { MemberId = standbymemberid, Standbynumber = standbynumber, StartboatId = startboat.StartboatId });
                    }
                }
                else if (!startboatstandbys.Any(e => e.Standbynumber == standbynumber))
                {
                    _context.StartboatStandbys.Add(new StartboatStandby { MemberId = standbymemberid, Standbynumber = standbynumber, StartboatId = startboat.StartboatId });
                }
            }
            else
            {
                var sbs = _context.StartboatStandbys.FirstOrDefault(e => e.Standbynumber == standbynumber && e.MemberId == oldstandbyid && e.StartboatId == startboat.StartboatId);

                if (sbs != null)
                {
                    _context.StartboatStandbys.Remove(sbs);
                }
            }

            _context.SaveChanges();
        }

        private int getAgeFrom(int MemberFromAge, bool withAllAvailable)
        {
            int yearnow = DateTime.Now.Year;
            int ageFrom = yearnow - MemberFromAge;

            if (withAllAvailable == true)
            {
                if (MemberFromAge == 0)
                {
                    ageFrom = yearnow - MemberFromAge;
                }
                else if (MemberFromAge == 7)
                {
                    ageFrom = yearnow - 0;
                }
                else if (MemberFromAge == 8)
                {
                    ageFrom = yearnow - 7;
                }
                else if (MemberFromAge == 9)
                {
                    ageFrom = yearnow - 7;
                }
                else if (MemberFromAge == 10)
                {
                    ageFrom = yearnow - 7;
                }
                else if (MemberFromAge == 11)
                {
                    ageFrom = yearnow - 9;
                }
                else if (MemberFromAge == 12)
                {
                    ageFrom = yearnow - 12;
                }
                else if (MemberFromAge == 13)
                {
                    ageFrom = yearnow - 9;
                }
                else if (MemberFromAge == 14)
                {
                    ageFrom = yearnow - 13;
                }
                else if (MemberFromAge == 15)
                {
                    ageFrom = yearnow - 13;
                }
                else if (MemberFromAge == 17)
                {
                    ageFrom = yearnow - 12;
                }
                else if (MemberFromAge == 19)
                {
                    ageFrom = yearnow - 17;
                }
                else if (MemberFromAge == 32)
                {
                    ageFrom = yearnow - MemberFromAge;
                }
                else if (MemberFromAge == 40)
                {
                    ageFrom = yearnow - MemberFromAge;
                }
                else if (MemberFromAge == 50)
                {
                    ageFrom = yearnow - MemberFromAge;
                }
            }

            return ageFrom;
        }

        private int getAgeTo(int MemberFromAge, int MemberToAge, bool withAllAvailable)
        {
            int yearnow = DateTime.Now.Year;
            int ageFrom = 0;
            int ageTo = yearnow - MemberToAge;

            if (withAllAvailable == true)
            {
                if (MemberFromAge == 19)
                {
                    ageFrom = yearnow - 17;
                    ageTo = yearnow - 49;
                }
                else if (MemberFromAge == 32)
                {
                    ageFrom = yearnow - MemberFromAge;
                    ageTo = yearnow - 99;
                }
                else if (MemberFromAge == 40)
                {
                    ageFrom = yearnow - MemberFromAge;
                    ageTo = yearnow - 99;
                }
                else if (MemberFromAge == 50)
                {
                    ageFrom = yearnow - MemberFromAge;
                    ageTo = yearnow - 99;
                }
            }

            return ageTo;
        }

        private int AddReportedStartboat(int id, int seat1, int seat2, int seat3, int seat4, int seat5, int seat6, int seat7, int seat8, int standby1, int standby2, int standby3, int standby4,
            int standby5, int standby6, int standby7, int standby8, bool standbycheck1, bool standbycheck2, bool standbycheck3, bool standbycheck4, bool standbycheck5,
            bool standbycheck6, bool standbycheck7, bool standbycheck8, int clubid, int seatnumber, int rid)
        {            
            _context.ReportedStartboats.Add(new ReportedStartboat { ClubId = clubid, ReportedRaceId = id, RegattaId = rid });

            List<int> seats = new List<int>();

            bool isDouble = false;

            if (seatnumber == 1)
            {
                if (seat1 == 0)
                {
                    return 1;
                }

                seats.Add(seat1);

                if (standbycheck1 == true)
                {
                    if (standby1 == 0)
                    {
                        return 1;
                    }
                    seats.Add(standby1);
                }

                if (seats.GroupBy(s => s).SelectMany(grp => grp.Skip(1)).Count() >= 1)
                {
                    isDouble = true;
                    return 2;
                }

                if (isDouble == false)
                {
                    _context.SaveChanges();
                    _context.ReportedStartboatMembers.Add(new ReportedStartboatMember { MemberId = seat1, Seatnumber = 1, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });

                    if (standbycheck1 == true)
                    {
                        _context.ReportedStartboatStandbys.Add(new ReportedStartboatStandby { MemberId = standby1, Standbynumber = 1, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    }

                    _context.SaveChanges();
                }
            }
            else if (seatnumber == 2)
            {
                if (seat1 == 0 || seat2 == 0)
                {
                    return 1;
                }

                seats.Add(seat1);
                seats.Add(seat2);

                if (standbycheck1 == true)
                {
                    if (standby1 == 0)
                    {
                        return 1;
                    }
                    seats.Add(standby1);
                }
                if (standbycheck2 == true)
                {
                    if (standby2 == 0)
                    {
                        return 1;
                    }
                    seats.Add(standby2);
                }

                if (seats.GroupBy(s => s).SelectMany(grp => grp.Skip(1)).Count() >= 1)
                {
                    isDouble = true;
                    return 2;
                }

                if (isDouble == false)
                {
                    _context.SaveChanges();

                    if (standbycheck1 == true)
                    {
                        _context.ReportedStartboats.Include(e => e.ReportedStartboatStandbys).Last().ReportedStartboatStandbys.Add(new ReportedStartboatStandby { MemberId = standby1, Standbynumber = 1, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    }
                    if (standbycheck2 == true)
                    {
                        _context.ReportedStartboats.Include(e => e.ReportedStartboatStandbys).Last().ReportedStartboatStandbys.Add(new ReportedStartboatStandby { MemberId = standby2, Standbynumber = 2, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    }
                    _context.ReportedStartboatMembers.Add(new ReportedStartboatMember { MemberId = seat1, Seatnumber = 1, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    _context.ReportedStartboatMembers.Add(new ReportedStartboatMember { MemberId = seat2, Seatnumber = 2, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });

                    _context.SaveChanges();
                }
            }
            else if (seatnumber == 4)
            {
                if (seat1 == 0 || seat2 == 0 || seat3 == 0 || seat4 == 0)
                {
                    return 1;
                }

                seats.Add(seat1);
                seats.Add(seat2);
                seats.Add(seat3);
                seats.Add(seat4);

                if (standbycheck1 == true)
                {
                    if (standby1 == 0)
                    {
                        return 1;
                    }
                    seats.Add(standby1);
                }
                if (standbycheck2 == true)
                {
                    if (standby2 == 0)
                    {
                        return 1;
                    }
                    seats.Add(standby2);
                }
                if (standbycheck3 == true)
                {
                    if (standby3 == 0)
                    {
                        return 1;
                    }
                    seats.Add(standby3);
                }
                if (standbycheck4 == true)
                {
                    if (standby4 == 0)
                    {
                        return 1;
                    }
                    seats.Add(standby4);
                }

                if (seats.GroupBy(s => s).SelectMany(grp => grp.Skip(1)).Count() >= 1)
                {
                    isDouble = true;
                    return 2;
                }

                if (isDouble == false)
                {
                    _context.SaveChanges();

                    _context.ReportedStartboats.Include(e => e.ReportedStartboatMembers).Last().ReportedStartboatMembers.Add(new ReportedStartboatMember { MemberId = seat1, Seatnumber = 1, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    _context.ReportedStartboats.Include(e => e.ReportedStartboatMembers).Last().ReportedStartboatMembers.Add(new ReportedStartboatMember { MemberId = seat2, Seatnumber = 2, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    _context.ReportedStartboats.Include(e => e.ReportedStartboatMembers).Last().ReportedStartboatMembers.Add(new ReportedStartboatMember { MemberId = seat3, Seatnumber = 3, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    _context.ReportedStartboats.Include(e => e.ReportedStartboatMembers).Last().ReportedStartboatMembers.Add(new ReportedStartboatMember { MemberId = seat4, Seatnumber = 4, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });

                    if (standbycheck1 == true)
                    {
                        _context.ReportedStartboats.Include(e => e.ReportedStartboatStandbys).Last().ReportedStartboatStandbys.Add(new ReportedStartboatStandby { MemberId = standby1, Standbynumber = 1, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    }
                    if (standbycheck2 == true)
                    {
                        _context.ReportedStartboats.Include(e => e.ReportedStartboatStandbys).Last().ReportedStartboatStandbys.Add(new ReportedStartboatStandby { MemberId = standby2, Standbynumber = 2, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    }
                    if (standbycheck3 == true)
                    {
                        _context.ReportedStartboats.Include(e => e.ReportedStartboatStandbys).Last().ReportedStartboatStandbys.Add(new ReportedStartboatStandby { MemberId = standby3, Standbynumber = 3, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    }
                    if (standbycheck4 == true)
                    {
                        _context.ReportedStartboats.Include(e => e.ReportedStartboatStandbys).Last().ReportedStartboatStandbys.Add(new ReportedStartboatStandby { MemberId = standby4, Standbynumber = 4, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    }

                    _context.SaveChanges();
                }
            }
            else if (seatnumber == 8)
            {
                if (seat1 == 0 || seat2 == 0 || seat3 == 0 || seat4 == 0 || seat5 == 0 || seat6 == 0 || seat7 == 0 || seat8 == 0)
                {
                    return 1;
                }

                seats.Add(seat1);
                seats.Add(seat2);
                seats.Add(seat3);
                seats.Add(seat4);
                seats.Add(seat5);
                seats.Add(seat6);
                seats.Add(seat7);
                seats.Add(seat8);

                if (standbycheck1 == true)
                {
                    if (standby1 == 0)
                    {
                        return 1;
                    }
                    seats.Add(standby1);
                }
                if (standbycheck2 == true)
                {
                    if (standby2 == 0)
                    {
                        return 1;
                    }
                    seats.Add(standby2);
                }
                if (standbycheck3 == true)
                {
                    if (standby3 == 0)
                    {
                        return 1;
                    }
                    seats.Add(standby3);
                }
                if (standbycheck4 == true)
                {
                    if (standby4 == 0)
                    {
                        return 1;
                    }
                    seats.Add(standby4);
                }
                if (standbycheck5 == true)
                {
                    if (standby5 == 0)
                    {
                        return 1;
                    }
                    seats.Add(standby5);
                }
                if (standbycheck6 == true)
                {
                    if (standby6 == 0)
                    {
                        return 1;
                    }
                    seats.Add(standby6);
                }
                if (standbycheck7 == true)
                {
                    if (standby7 == 0)
                    {
                        return 1;
                    }
                    seats.Add(standby7);
                }
                if (standbycheck8 == true)
                {
                    if (standby8 == 0)
                    {
                        return 1;
                    }
                    seats.Add(standby8);
                }

                if (seats.GroupBy(s => s).SelectMany(grp => grp.Skip(1)).Count() >= 1)
                {
                    isDouble = true;
                    return 2;
                }

                if (isDouble == false)
                {
                    _context.SaveChanges();
                    _context.ReportedStartboats.Include(e => e.ReportedStartboatMembers).Last().ReportedStartboatMembers.Add(new ReportedStartboatMember { MemberId = seat1, Seatnumber = 1, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    _context.ReportedStartboats.Include(e => e.ReportedStartboatMembers).Last().ReportedStartboatMembers.Add(new ReportedStartboatMember { MemberId = seat2, Seatnumber = 2, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    _context.ReportedStartboats.Include(e => e.ReportedStartboatMembers).Last().ReportedStartboatMembers.Add(new ReportedStartboatMember { MemberId = seat3, Seatnumber = 3, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    _context.ReportedStartboats.Include(e => e.ReportedStartboatMembers).Last().ReportedStartboatMembers.Add(new ReportedStartboatMember { MemberId = seat4, Seatnumber = 4, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    _context.ReportedStartboats.Include(e => e.ReportedStartboatMembers).Last().ReportedStartboatMembers.Add(new ReportedStartboatMember { MemberId = seat5, Seatnumber = 5, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    _context.ReportedStartboats.Include(e => e.ReportedStartboatMembers).Last().ReportedStartboatMembers.Add(new ReportedStartboatMember { MemberId = seat6, Seatnumber = 6, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    _context.ReportedStartboats.Include(e => e.ReportedStartboatMembers).Last().ReportedStartboatMembers.Add(new ReportedStartboatMember { MemberId = seat7, Seatnumber = 7, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    _context.ReportedStartboats.Include(e => e.ReportedStartboatMembers).Last().ReportedStartboatMembers.Add(new ReportedStartboatMember { MemberId = seat8, Seatnumber = 8, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });

                    if (standbycheck1 == true)
                    {
                        _context.ReportedStartboats.Include(e => e.ReportedStartboatStandbys).Last().ReportedStartboatStandbys.Add(new ReportedStartboatStandby { MemberId = standby1, Standbynumber = 1, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    }
                    if (standbycheck2 == true)
                    {
                        _context.ReportedStartboats.Include(e => e.ReportedStartboatStandbys).Last().ReportedStartboatStandbys.Add(new ReportedStartboatStandby { MemberId = standby2, Standbynumber = 2, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    }
                    if (standbycheck3 == true)
                    {
                        _context.ReportedStartboats.Include(e => e.ReportedStartboatStandbys).Last().ReportedStartboatStandbys.Add(new ReportedStartboatStandby { MemberId = standby3, Standbynumber = 3, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    }
                    if (standbycheck4 == true)
                    {
                        _context.ReportedStartboats.Include(e => e.ReportedStartboatStandbys).Last().ReportedStartboatStandbys.Add(new ReportedStartboatStandby { MemberId = standby4, Standbynumber = 4, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    }
                    if (standbycheck5 == true)
                    {
                        _context.ReportedStartboats.Include(e => e.ReportedStartboatStandbys).Last().ReportedStartboatStandbys.Add(new ReportedStartboatStandby { MemberId = standby5, Standbynumber = 5, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    }
                    if (standbycheck6 == true)
                    {
                        _context.ReportedStartboats.Include(e => e.ReportedStartboatStandbys).Last().ReportedStartboatStandbys.Add(new ReportedStartboatStandby { MemberId = standby6, Standbynumber = 6, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    }
                    if (standbycheck7 == true)
                    {
                        _context.ReportedStartboats.Include(e => e.ReportedStartboatStandbys).Last().ReportedStartboatStandbys.Add(new ReportedStartboatStandby { MemberId = standby7, Standbynumber = 7, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    }
                    if (standbycheck8 == true)
                    {
                        _context.ReportedStartboats.Include(e => e.ReportedStartboatStandbys).Last().ReportedStartboatStandbys.Add(new ReportedStartboatStandby { MemberId = standby8, Standbynumber = 8, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    }

                    _context.SaveChanges();
                }
            }

            return 0;
        }

        private bool TryDrawRaces(int id)
        {
            var race = _context.Races.FirstOrDefault(e => e.RaceId == id);
            var racedraw = _context.RaceDraws.FirstOrDefault(e => e.RaceDrawId == race.RaceDrawId);
            var torace = _context.Races.FirstOrDefault(e => e.ReportedRaceId == race.ReportedRaceId && e.RaceTypId == 4);
            int i = 1;

            if (race.RaceTypId == 1 || race.RaceTypId == 2 || race.RaceTypId == 3)
            {
                var rtid = race.RaceTypId;
                if (!_context.Races.Any(e => e.RacestatusId != 3 && e.ReportedRaceId == race.ReportedRaceId && e.RaceTypId == rtid))
                {
                    var finishedraces = _context.Races.Where(e => e.ReportedRaceId == race.ReportedRaceId && e.RacestatusId == 3 && e.RaceTypId == rtid).ToList();
                    var racedrawrules = _context.RaceDrawRules.Where(e => e.RaceDrawId == race.RaceDrawId && e.RaceTypId == race.RaceTypId && e.RaceSequence == race.Sequence).ToList();
                    var startboats = _context.Startboats.Where(e => e.RaceId == race.RaceId).ToList();
                    var startboatmembers = _context.StartboatMembers.ToList();

                    foreach (var fr in finishedraces)
                    {
                        startboats = _context.Startboats.Where(e => e.RaceId == fr.RaceId).ToList();
                        racedrawrules = _context.RaceDrawRules.Where(e => e.RaceDrawId == fr.RaceDrawId && e.RaceTypId == fr.RaceTypId && e.RaceSequence == fr.Sequence).ToList();                        
                        foreach (var rdr in racedrawrules)
                        {
                            foreach (var sb in startboats)
                            {
                                if (sb.Placement >= rdr.PlacementFrom && sb.Placement <= rdr.PlacementTo)
                                {
                                    torace = _context.Races.FirstOrDefault(e => e.ReportedRaceId == race.ReportedRaceId && e.RaceTypId == rdr.ToRaceTypId && e.Sequence == rdr.ToRaceSequence);
                                    if (torace != null)
                                    {
                                        i = _context.Startboats.Where(e => e.RaceId == torace.RaceId).Count();

                                        if(!(torace.RaceTypId == 4 && _context.Races.Any(e => e.ReportedRaceId == torace.ReportedRaceId && (e.RaceTypId == 2 || e.RaceTypId == 3) && e.RacestatusId != 3))) 
                                        {
                                            torace.RacestatusId = 1004;
                                        }                                        
                                        _context.Races.Update(torace);

                                        _context.Startboats.Add(new Startboat { ClubId = sb.ClubId, RaceId = torace.RaceId, StartboatstatusId = 6, Startslot = i+1, RegattaId = race.RegattaId, Gender = race.Gender, ReportedStartboatId = sb.ReportedStartboatId });
                                        _context.SaveChanges();

                                        startboatmembers = _context.StartboatMembers.Where(e => e.StartboatId == sb.StartboatId).ToList();
                                        foreach (var sbm in startboatmembers)
                                        {
                                            _context.StartboatMembers.Add(new StartboatMember { MemberId = sbm.MemberId, SeatNumber = sbm.SeatNumber, StartboatId = _context.Startboats.Where(x => x.RaceId == torace.RaceId && x.Startslot == i+1).First().StartboatId });
                                        }
                                        _context.SaveChanges();                                        
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }        
    }
}