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
                var reportedStartboats = _context.ReportedStartboats.Include(e => e.ReportedRace).ThenInclude(e => e.Competition).ToList();
                var startingFees = _context.StartingFees.ToList();
                var campingFees = _context.CampingFees.ToList();
                var oldclasses = _context.Oldclasses.ToList();

                ViewBag.reportedStartboats = reportedStartboats;
                ViewBag.startingFees = startingFees;
                ViewBag.oldclasses = oldclasses;
                ViewBag.rid = rid;

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

            var reportedStartboats = _context.ReportedStartboats.Where(e => e.ClubId == id).ToList();
            var startingFees = _context.StartingFees.ToList();
            var oldclasses = _context.Oldclasses.ToList();

            ViewBag.reportedStartboats = reportedStartboats;
            ViewBag.startingFees = startingFees;
            ViewBag.oldclasses = oldclasses;
            ViewBag.rid = rid;

            return View();
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