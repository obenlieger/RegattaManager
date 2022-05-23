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
using RegattaManager.ViewModels;

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
                var reportedStartboats = _context.ReportedStartboats.Include(e => e.ReportedStartboatMembers).Include(e => e.ReportedRace).ThenInclude(e => e.Competition).Where(e => 
                    !e.ReportedStartboatMembers.Select(x => x.MemberId).Contains(1)
                    && !e.ReportedStartboatMembers.Select(x => x.MemberId).Contains(2)
                    && !e.ReportedStartboatMembers.Select(x => x.MemberId).Contains(3)
                    && !e.ReportedStartboatMembers.Select(x => x.MemberId).Contains(4)
                    && !e.ReportedStartboatMembers.Select(x => x.MemberId).Contains(5)
                    && !e.ReportedStartboatMembers.Select(x => x.MemberId).Contains(6)
                    && !e.ReportedStartboatMembers.Select(x => x.MemberId).Contains(7)
                    && !e.ReportedStartboatMembers.Select(x => x.MemberId).Contains(8)).ToList();
                var startingFees = _context.StartingFees.ToList();
                var campingFees = _context.CampingFees.ToList();
                var oldclasses = _context.Oldclasses.ToList();
                var reportedstartboatmembers = _context.ReportedStartboatMembers.Select(e => e.MemberId).Distinct().ToList();
                var members = _context.Members.Where(e => reportedstartboatmembers.Contains(e.MemberId)).ToList();

                ViewBag.reportedStartboats = reportedStartboats;
                ViewBag.startingFees = startingFees;
                ViewBag.oldclasses = oldclasses;
                ViewBag.members = members;
                ViewBag.rid = rid;
                ViewBag.subscriberfee = _context.Regattas.FirstOrDefault(e => e.RegattaId == rid).SubscriberFee;

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

            var reportedStartboats = _context.ReportedStartboats.Include(e => e.ReportedStartboatMembers).Include(e => e.ReportedRace).ThenInclude(e => e.Competition).Where(e => e.ClubId == id 
                && !e.ReportedStartboatMembers.Select(x => x.MemberId).Contains(1)
                && !e.ReportedStartboatMembers.Select(x => x.MemberId).Contains(2)
                && !e.ReportedStartboatMembers.Select(x => x.MemberId).Contains(3)
                && !e.ReportedStartboatMembers.Select(x => x.MemberId).Contains(4)
                && !e.ReportedStartboatMembers.Select(x => x.MemberId).Contains(5)
                && !e.ReportedStartboatMembers.Select(x => x.MemberId).Contains(6)
                && !e.ReportedStartboatMembers.Select(x => x.MemberId).Contains(7)
                && !e.ReportedStartboatMembers.Select(x => x.MemberId).Contains(8)).ToList();
            var startingFees = _context.StartingFees.Include(e => e.Boatclasses).OrderBy(e => e.Boatclasses.Name).ToList();
            var oldclasses = _context.Oldclasses.ToList();

            ViewBag.reportedStartboats = reportedStartboats;
            ViewBag.startingFees = startingFees;
            ViewBag.oldclasses = oldclasses;
            ViewBag.rid = rid;

            return View(model);
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

        public IActionResult Mannschaftswertung()
        {
            var regattaclubs = _context.RegattaClubs.ToList();
            var races = _context.Races.Where(e => e.RaceTypId == 4 && e.RacestatusId == 3).ToList();
            var clubs = _context.Clubs.Where(e => regattaclubs.Select(i => i.ClubId).Contains(e.ClubId));            
            var startboats = _context.Startboats.Include(e => e.Race).ThenInclude(e => e.Boatclass).Where(e => e.Race.RaceTypId == 4 && e.StartboatstatusId == 3).OrderBy(e => e.Race.Oldclass.FromAge).ToList();
            List<StartboatMember> sbmembers = new List<StartboatMember>();
            List<int> addedsbid = new List<int>();
            List<Mannschaftswertung> mw = new List<Mannschaftswertung>();
            var oldclasses = _context.Oldclasses.OrderBy(e => e.FromAge).ToList();
            var tempsbm = _context.StartboatMembers.ToList();

            List<StartboatMember> addedmembers = new List<StartboatMember>();

            double wertung = 0;

            double schuelercb10 = 0;
            double schuelerb = 0;
            double schuelera = 0;
            double jugend = 0;
            double junioren = 0;
            double leistungsklasse = 0;
            double senioren = 0;

            int t = 0;

            foreach (var c in clubs)                
            {
                schuelercb10 = 0;
                schuelerb = 0;
                schuelera = 0;
                jugend = 0;
                junioren = 0;
                leistungsklasse = 0;
                senioren = 0;

                tempsbm = _context.StartboatMembers.Include(e => e.Member).Where(e => e.Member.ClubId == c.ClubId).ToList();
                races = _context.Races.Where(e => e.RaceTypId == 4 && e.RacestatusId == 3).ToList();
                startboats = _context.Startboats.Include(e => e.Race).ThenInclude(e => e.Boatclass).Where(e => races.Select(i => i.RaceId).Contains(e.RaceId) && tempsbm.Select(x => x.StartboatId).Contains(e.StartboatId)).ToList();

                foreach (var sb in startboats)                    
                {                                    
                    foreach (var oc in oldclasses)
                    {
                        wertung = 0;
                        sbmembers = _context.StartboatMembers.Include(e => e.Member).Include(e => e.Startboat).ThenInclude(e => e.Race).Where(e => e.StartboatId == sb.StartboatId && e.Member.ClubId == c.ClubId && e.Startboat.Race.OldclassId == oc.OldclassId).ToList();
                        if(sbmembers != null)
                        {
                            foreach (var sbm in sbmembers)
                            {
                                if (!addedmembers.Contains(sbm))
                                {
                                    if (sbm.Startboat.Race.RaceDrawId == 1 || sbm.Startboat.Race.RaceDrawId == 9)
                                    {
                                        if (sbm.Startboat.Placement == 1)
                                        {
                                            t = 0;
                                            t = 7 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 2)
                                        {
                                            t = 0;
                                            t = 5 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 3)
                                        {
                                            t = 0;
                                            t = 4 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 4)
                                        {
                                            t = 0;
                                            t = 3 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 5)
                                        {
                                            t = 0;
                                            t = 2 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 6)
                                        {
                                            t = 0;
                                            t = 1 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                    }
                                    else if (sbm.Startboat.Race.RaceDrawId == 2 || sbm.Startboat.Race.RaceDrawId == 10)
                                    {
                                        if (sbm.Startboat.Placement == 1)
                                        {
                                            t = 0;
                                            t = 8 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 2)
                                        {
                                            t = 0;
                                            t = 6 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 3)
                                        {
                                            t = 0;
                                            t = 5 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 4)
                                        {
                                            t = 0;
                                            t = 4 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 5)
                                        {
                                            t = 0;
                                            t = 3 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 6)
                                        {
                                            t = 0;
                                            t = 2 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                    }
                                    else if (sbm.Startboat.Race.RaceDrawId == 3 || sbm.Startboat.Race.RaceDrawId == 11)
                                    {
                                        if (sbm.Startboat.Placement == 1)
                                        {
                                            t = 0;
                                            t = 9 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 2)
                                        {
                                            t = 0;
                                            t = 7 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 3)
                                        {
                                            t = 0;
                                            t = 6 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 4)
                                        {
                                            t = 0;
                                            t = 5 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 5)
                                        {
                                            t = 0;
                                            t = 4 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 6)
                                        {
                                            t = 0;
                                            t = 3 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                    }
                                    else if (sbm.Startboat.Race.RaceDrawId == 4 || sbm.Startboat.Race.RaceDrawId == 12)
                                    {
                                        if (sbm.Startboat.Placement == 1)
                                        {
                                            t = 0;
                                            t = 10 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 2)
                                        {
                                            t = 0;
                                            t = 8 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 3)
                                        {
                                            t = 0;
                                            t = 7 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 4)
                                        {
                                            t = 0;
                                            t = 6 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 5)
                                        {
                                            t = 0;
                                            t = 5 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 6)
                                        {
                                            t = 0;
                                            t = 4 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                    }
                                    else if (sbm.Startboat.Race.RaceDrawId == 5 || sbm.Startboat.Race.RaceDrawId == 13)
                                    {
                                        if (sbm.Startboat.Placement == 1)
                                        {
                                            t = 0;
                                            t = 11 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 2)
                                        {
                                            t = 0;
                                            t = 9 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 3)
                                        {
                                            t = 0;
                                            t = 8 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 4)
                                        {
                                            t = 0;
                                            t = 7 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 5)
                                        {
                                            t = 0;
                                            t = 6 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 6)
                                        {
                                            t = 0;
                                            t = 5 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                    }
                                    else if (sbm.Startboat.Race.RaceDrawId == 6 || sbm.Startboat.Race.RaceDrawId == 14)
                                    {
                                        if (sbm.Startboat.Placement == 1)
                                        {
                                            t = 0;
                                            t = 12 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 2)
                                        {
                                            t = 0;
                                            t = 10 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 3)
                                        {
                                            t = 0;
                                            t = 9 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 4)
                                        {
                                            t = 0;
                                            t = 8 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 5)
                                        {
                                            t = 0;
                                            t = 7 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 6)
                                        {
                                            t = 0;
                                            t = 6 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                    }
                                    else if (sbm.Startboat.Race.RaceDrawId == 7)
                                    {
                                        if (sbm.Startboat.Placement == 1)
                                        {
                                            t = 0;
                                            t = 13 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 2)
                                        {
                                            t = 0;
                                            t = 11 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 3)
                                        {
                                            t = 0;
                                            t = 10 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 4)
                                        {
                                            t = 0;
                                            t = 9 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 5)
                                        {
                                            t = 0;
                                            t = 8 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 6)
                                        {
                                            t = 0;
                                            t = 7 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                    }
                                    else if (sbm.Startboat.Race.RaceDrawId == 8)
                                    {
                                        if (sbm.Startboat.Placement == 1)
                                        {
                                            t = 0;
                                            t = 14 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 2)
                                        {
                                            t = 0;
                                            t = 12 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 3)
                                        {
                                            t = 0;
                                            t = 11 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 4)
                                        {
                                            t = 0;
                                            t = 10 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 5)
                                        {
                                            t = 0;
                                            t = 9 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                        if (sbm.Startboat.Placement == 6)
                                        {
                                            t = 0;
                                            t = 8 / sb.Race.Boatclass.Seats;

                                            wertung = wertung + t;
                                        }
                                    }
                                }                                
                            }
                        }
                                                                                
                        if (oc.ToAge <= 10)
                        {
                            schuelercb10 = schuelercb10 + wertung;
                        }
                        else if (oc.FromAge >= 11 && oc.ToAge <= 12)
                        {
                            schuelerb = schuelerb + wertung;
                        }
                        else if (oc.FromAge >= 13 && oc.ToAge <= 14)
                        {
                            schuelera = schuelera + wertung;
                        }
                        else if (oc.FromAge >= 15 && oc.ToAge <= 16)
                        {
                            jugend = jugend + wertung;
                        }
                        else if (oc.FromAge >= 17 && oc.ToAge <= 18)
                        {
                            junioren = junioren + wertung;
                        }
                        else if (oc.FromAge >= 19 && oc.ToAge <= 31)
                        {
                            leistungsklasse = leistungsklasse + wertung;
                        }
                        else if (oc.FromAge >= 32)
                        {
                            senioren = senioren + wertung;
                        }
                        addedsbid.Add(sb.StartboatId);
                    }
                }

                if(schuelercb10 > 0)
                {
                    mw.Add(new Mannschaftswertung { ClubId = c.ClubId, ClubName = c.Name, OldclassName = "Schüler C/B10", Wertung = schuelercb10 });
                }
                if(schuelerb > 0)
                {
                    mw.Add(new Mannschaftswertung { ClubId = c.ClubId, ClubName = c.Name, OldclassName = "Schüler B", Wertung = schuelerb });
                }
                if(schuelera > 0)
                {
                    mw.Add(new Mannschaftswertung { ClubId = c.ClubId, ClubName = c.Name, OldclassName = "Schüler A", Wertung = schuelera });
                }
                if(jugend > 0)
                {
                    mw.Add(new Mannschaftswertung { ClubId = c.ClubId, ClubName = c.Name, OldclassName = "Jugend", Wertung = jugend });
                }
                if(junioren > 0)
                {
                    mw.Add(new Mannschaftswertung { ClubId = c.ClubId, ClubName = c.Name, OldclassName = "Junioren", Wertung = junioren });
                }
                if(leistungsklasse > 0)
                {
                    mw.Add(new Mannschaftswertung { ClubId = c.ClubId, ClubName = c.Name, OldclassName = "Leistungsklasse", Wertung = leistungsklasse });
                }
                if(senioren > 0)
                {
                    mw.Add(new Mannschaftswertung { ClubId = c.ClubId, ClubName = c.Name, OldclassName = "Senioren", Wertung = senioren });
                }                
            }            

            return View(mw);
        }

        private ClubBill populateClubBill(int? id)
        {
            ClubBill clubbill = new ClubBill();
            var club = _context.Clubs.FirstOrDefault(e => e.ClubId == id);
            var rsb = _context.ReportedStartboats.Include(e => e.ReportedRace).ThenInclude(e => e.Competition).Where(e => e.ClubId == id).ToList();
            var members = _context.Members.Where(e => e.ClubId == id).ToList();
            var repmembers = _context.ReportedStartboatMembers.Where(e => members.Select(i => i.MemberId).Contains(e.MemberId)).ToList();
            var clubmembers = _context.Members.Where(e => repmembers.Select(i => i.MemberId).Contains(e.MemberId)).ToList();
            var regatta = _context.Regattas.FirstOrDefault(e => e.RegattaId == rsb.First().RegattaId);
            var regattastartingfees = _context.RegattaStartingFees.Where(e => e.RegattaId == regatta.RegattaId).ToList();
            var startingfees = _context.StartingFees.Where(e => regattastartingfees.Select(i => i.StartingFeeId).Contains(e.StartingFeeId)).ToList();
            var oldclasses = _context.Oldclasses.ToList();
            int fromoc = 0;
            int tooc = 0;
            double sbfee = 0;

            foreach(var sf in startingfees)
            {
                foreach (var foc in oldclasses)
                {
                    if (sf.FromOldclassId == foc.OldclassId)
                    {
                        fromoc = foc.FromAge;
                    }
                    if (sf.ToOldclassId == foc.OldclassId)
                    {
                        tooc = foc.ToAge;
                    }
                }
                foreach (var sbf in rsb)
                {
                    if (sbf.ReportedRace.Competition.BoatclassId == sf.BoatclassId)
                    {
                        foreach (var oc in oldclasses)
                        {
                            if (sbf.ReportedRace.OldclassId == oc.OldclassId)
                            {
                                if (oc.FromAge >= fromoc && oc.ToAge <= tooc && sbf.ClubId == club.ClubId && sbf.RegattaId == ViewBag.rid)
                                {
                                    sbfee = sbfee + sf.Amount;
                                }
                            }
                        }
                    }
                }
            }

            clubbill.ClubName = club.Name;
            clubbill.members = clubmembers;
            clubbill.reportedStartboats = rsb;
            clubbill.StartboatCount = rsb.Count;
            clubbill.SubscriberFee = regatta.SubscriberFee * clubmembers.Count;
            clubbill.StartingFee = sbfee;

            return clubbill;
        }
    }
}