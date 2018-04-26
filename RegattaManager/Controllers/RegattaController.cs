using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RegattaManager.Data;
using RegattaManager.Models;
using RegattaManager.ViewModels;
using RegattaManager.Extensions;

namespace RegattaManager.Controllers
{
    [Authorize]
    public class RegattaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegattaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Regatta
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Regattas.Include(r => r.Club);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Regatta/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var regatta = await _context.Regattas
                .Include(r => r.Club)
                .Include(w => w.Waters)
                .SingleOrDefaultAsync(m => m.RegattaId == id);
            if (regatta == null)
            {
                return NotFound();
            }
                        
            return View(regatta);
        }

        // GET: Regatta/Create
        public IActionResult Create()
        {
            ViewData["ClubId"] = new SelectList(_context.Clubs.OrderBy(e => e.Name), "ClubId", "Name");
            ViewData["WaterId"] = new SelectList(_context.Waters.OrderBy(e => e.Name), "WaterId", "Name");
            ViewData["OldclassIds"] = new MultiSelectList(_context.Oldclasses.OrderBy(e => e.Name), "OldclassId", "Name");
            ViewData["CompetitionIds"] = new MultiSelectList(_context.Competitions.Include(e => e.Boatclasses).Include(e => e.Raceclasses), "CompetitionId", "Name");
            ViewData["StartingFeeIds"] = new MultiSelectList(_context.StartingFees, "StartingFeeId", "Name");
            ViewData["CampingFeeIds"] = new MultiSelectList(_context.CampingFees, "CampingFeeId", "LongName");
            return View();
        }

        // POST: Regatta/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RegattaId,RegattaName,RegattaVon,RegattaBis,Waterdepth,Startslots,ReportText,ReportSchedule,ReportOpening," +
            "ReportAddress,ReportTel,ReportFax,Judge,Awards,Security,ScheduleText,SubscriberFee,Accomodation,Comment,Catering,ClubId,WaterId")] RegattaVM regattaVM,
            IEnumerable<int> OldclassIds, IEnumerable<int> CompetitionIds, IEnumerable<int> StartingFeeIds, IEnumerable<int> CampingFeeIds)
        {
            Regatta regatta = new Regatta();

            regatta.Name = regattaVM.RegattaName;
            regatta.FromDate = regattaVM.RegattaVon;
            regatta.ToDate = regattaVM.RegattaBis;
            regatta.Waterdepth = regattaVM.Waterdepth;
            regatta.Startslots = regattaVM.Startslots;
            regatta.ReportText = regattaVM.ReportText;
            regatta.ReportSchedule = regattaVM.ReportSchedule;
            regatta.ReportOpening = regattaVM.ReportOpening;
            regatta.ReportAddress = regattaVM.ReportAddress;
            regatta.ReportTel = regattaVM.ReportTel;
            regatta.ReportFax = regattaVM.ReportFax;
            regatta.Judge = regattaVM.Judge;
            regatta.Awards = regattaVM.Awards;
            regatta.Security = regattaVM.Security;
            regatta.ScheduleText = regattaVM.ScheduleText;
            regatta.SubscriberFee = regattaVM.SubscriberFee;
            regatta.Accomodation = regattaVM.Accomodation;
            regatta.Comment = regattaVM.Comment;
            regatta.ClubId = regattaVM.ClubId;
            regatta.WaterId = regattaVM.WaterId;

            if (ModelState.IsValid)
            {
                _context.Add(regatta);
                await _context.SaveChangesAsync();

                IEnumerable<RegattaOldclass> roc = _context.RegattaOldclasses.Where(e => e.RegattaId == regattaVM.RegattaId);
                IEnumerable<RegattaCampingFee> rcf = _context.RegattaCampingFees.Where(e => e.RegattaId == regattaVM.RegattaId);
                IEnumerable<RegattaCompetition> rc = _context.RegattaCompetitions.Where(e => e.RegattaId == regattaVM.RegattaId);
                IEnumerable<RegattaStartingFee> rsf = _context.RegattaStartingFees.Where(e => e.RegattaId == regattaVM.RegattaId);

                var reg = _context.Regattas.Last();

                foreach (var oc in OldclassIds)
                {
                    _context.Regattas.Include(e => e.RegattaOldclasses).FirstOrDefault(m => m.RegattaId == reg.RegattaId).RegattaOldclasses.Add(new RegattaOldclass { RegattaId = regattaVM.RegattaId, OldclassId = oc });
                }

                foreach (var cf in CampingFeeIds)
                {
                    _context.Regattas.Include(e => e.RegattaCampingFees).FirstOrDefault(m => m.RegattaId == reg.RegattaId).RegattaCampingFees.Add(new RegattaCampingFee { RegattaId = regattaVM.RegattaId, CampingFeeId = cf });
                }

                foreach (var rcid in CompetitionIds)
                {
                    _context.Regattas.Include(e => e.RegattaCompetitions).FirstOrDefault(m => m.RegattaId == reg.RegattaId).RegattaCompetitions.Add(new RegattaCompetition { RegattaId = regattaVM.RegattaId, CompetitionId = rcid });
                }

                foreach (var rsfid in StartingFeeIds)
                {

                    _context.Regattas.Include(e => e.RegattaStartingFees).FirstOrDefault(m => m.RegattaId == reg.RegattaId).RegattaStartingFees.Add(new RegattaStartingFee { RegattaId = regattaVM.RegattaId, StartingFeeId = rsfid });
                }

                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            ViewData["ClubId"] = new SelectList(_context.Clubs, "ClubId", "Name", regatta.ClubId);
            ViewData["WaterId"] = new SelectList(_context.Waters, "WaterId", "Name", regatta.WaterId);
            ViewData["OldclassIds"] = new MultiSelectList(_context.Oldclasses, "OldclassId", "Name");
            ViewData["CompetitionIds"] = new MultiSelectList(_context.Competitions, "CompetitionId", "Name");
            ViewData["StartingFeeIds"] = new MultiSelectList(_context.StartingFees, "StartingFeeId", "Name");
            ViewData["CampingFeeIds"] = new MultiSelectList(_context.CampingFees, "CampingFeeId", "LongName");
            return View(regattaVM);
        }

        // GET: Regatta/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RegattaVM rvm = populateRegattaVM(id);

            if (rvm == null)
            {
                return NotFound();
            }
            ViewData["ClubId"] = new SelectList(_context.Clubs, "ClubId", "Name", rvm.ClubId);
            ViewData["WaterId"] = new SelectList(_context.Waters, "WaterId", "Name", rvm.WaterId);            
            ViewData["OldclassIds"] = new MultiSelectList(rvm.Oldclasses, "OldclassId", "Name", rvm.RegattaOldclasses.Select(e => e.OldclassId).ToList());
            ViewData["CompetitionIds"] = new MultiSelectList(rvm.Competitions, "CompetitionId", "Name", rvm.RegattaCompetitions.Select(e => e.CompetitionId).ToList());
            ViewData["StartingFeeIds"] = new MultiSelectList(rvm.StartingFees, "StartingFeeId", "Name", rvm.RegattaStartingFees.Select(e => e.StartingFeeId).ToList());
            ViewData["CampingFeeIds"] = new MultiSelectList(rvm.CampingFees, "CampingFeeId", "LongName", rvm.RegattaCampingFees.Select(e => e.CampingFeeId).ToList());
            return View(rvm);
        }

        // POST: Regatta/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RegattaId,RegattaName,RegattaVon,RegattaBis,Waterdepth,Startslots,ReportText,ReportSchedule,ReportOpening," +
            "ReportAddress,ReportTel,ReportFax,Judge,Awards,Security,ScheduleText,SubscriberFee,Accomodation,Comment,Catering,ClubId,WaterId")] RegattaVM regattaVM, 
            IEnumerable<int> OldclassIds, IEnumerable<int> CompetitionIds, IEnumerable<int> StartingFeeIds, IEnumerable<int> CampingFeeIds)
        {
            if (id != regattaVM.RegattaId)
            {
                return NotFound();
            }

            Regatta regatta = _context.Regattas.FirstOrDefault(e => e.RegattaId == regattaVM.RegattaId);

            regatta.Name = regattaVM.RegattaName;
            regatta.FromDate = regattaVM.RegattaVon;
            regatta.ToDate = regattaVM.RegattaBis;
            regatta.Waterdepth = regattaVM.Waterdepth;
            regatta.Startslots = regattaVM.Startslots;
            regatta.ReportText = regattaVM.ReportText;
            regatta.ReportSchedule = regattaVM.ReportSchedule;
            regatta.ReportOpening = regattaVM.ReportOpening;
            regatta.ReportAddress = regattaVM.ReportAddress;
            regatta.ReportTel = regattaVM.ReportTel;
            regatta.ReportFax = regattaVM.ReportFax;
            regatta.Judge = regattaVM.Judge;
            regatta.Awards = regattaVM.Awards;
            regatta.Security = regattaVM.Security;
            regatta.ScheduleText = regattaVM.ScheduleText;
            regatta.SubscriberFee = regattaVM.SubscriberFee;
            regatta.Accomodation = regattaVM.Accomodation;
            regatta.Comment = regattaVM.Comment;
            regatta.ClubId = regattaVM.ClubId;
            regatta.WaterId = regattaVM.WaterId;

            IEnumerable<RegattaOldclass> roc = _context.RegattaOldclasses.Where(e => e.RegattaId == regattaVM.RegattaId);
            IEnumerable<RegattaCampingFee> rcf = _context.RegattaCampingFees.Where(e => e.RegattaId == regattaVM.RegattaId);
            IEnumerable<RegattaCompetition> rc = _context.RegattaCompetitions.Where(e => e.RegattaId == regattaVM.RegattaId);
            IEnumerable<RegattaStartingFee> rsf = _context.RegattaStartingFees.Where(e => e.RegattaId == regattaVM.RegattaId);

            foreach(var oc in OldclassIds)
            {
                if(roc.Where(e => e.OldclassId == oc && e.RegattaId == regattaVM.RegattaId).Count() == 0)
                {
                    _context.Regattas.Include(e => e.RegattaOldclasses).FirstOrDefault(m => m.RegattaId == regattaVM.RegattaId).RegattaOldclasses.Add(new RegattaOldclass { RegattaId = regattaVM.RegattaId, OldclassId = oc });
                }                
            }

            foreach(var cf in CampingFeeIds)
            {
                if(rcf.Where(e => e.CampingFeeId == cf && e.RegattaId == regattaVM.RegattaId).Count() == 0)
                {
                    _context.Regattas.Include(e => e.RegattaCampingFees).FirstOrDefault(m => m.RegattaId == regattaVM.RegattaId).RegattaCampingFees.Add(new RegattaCampingFee { RegattaId = regattaVM.RegattaId, CampingFeeId = cf });
                }
            }

            foreach(var rcid in CompetitionIds)
            {
                if(rc.Where(e => e.CompetitionId == rcid && e.RegattaId == regattaVM.RegattaId).Count() == 0)
                {
                    _context.Regattas.Include(e => e.RegattaCompetitions).FirstOrDefault(m => m.RegattaId == regattaVM.RegattaId).RegattaCompetitions.Add(new RegattaCompetition { RegattaId = regattaVM.RegattaId, CompetitionId = rcid });
                }
            }

            foreach(var rsfid in StartingFeeIds)
            {
                if(rsf.Where(e => e.StartingFeeId == rsfid && e.RegattaId == regattaVM.RegattaId).Count() == 0)
                {
                    _context.Regattas.Include(e => e.RegattaStartingFees).FirstOrDefault(m => m.RegattaId == regattaVM.RegattaId).RegattaStartingFees.Add(new RegattaStartingFee { RegattaId = regattaVM.RegattaId, StartingFeeId = rsfid });
                }
            }

            _context.SaveChanges();
         
            foreach(var oldoc in roc)
            {
                if(!OldclassIds.Contains(oldoc.OldclassId))
                {
                    regatta.RegattaOldclasses.Remove(oldoc);
                }
            }

            foreach(var oldcf in rcf)
            {
                if(!CampingFeeIds.Contains(oldcf.CampingFeeId))
                {
                    regatta.RegattaCampingFees.Remove(oldcf);
                }
            }

            foreach(var oldrcid in rc)
            {
                if(!CompetitionIds.Contains(oldrcid.CompetitionId))
                {
                    regatta.RegattaCompetitions.Remove(oldrcid);
                }
            }

            foreach(var oldrsfid in rsf)
            {
                if(!StartingFeeIds.Contains(oldrsfid.StartingFeeId))
                {
                    regatta.RegattaStartingFees.Remove(oldrsfid);
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(regatta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegattaExists(regatta.RegattaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubId", regatta.ClubId);
            ViewData["WaterId"] = new SelectList(_context.Waters, "WaterId", "WaterId", regatta.WaterId);
            ViewData["RegattaOldclasses"] = new MultiSelectList(_context.Oldclasses, "OldclassId", "Name", regattaVM.RegattaOldclasses.Select(e => e.OldclassId).ToList());
            ViewData["RegattaCompetitions"] = new MultiSelectList(_context.Competitions, "CompetitionId", "Name", regattaVM.RegattaCompetitions.Select(e => e.CompetitionId).ToList());
            ViewData["RegattaStartingFees"] = new MultiSelectList(_context.StartingFees, "StartingFeeId", "Name", regattaVM.RegattaStartingFees.Select(e => e.StartingFeeId).ToList());
            ViewData["RegattaCampingFees"] = new MultiSelectList(_context.CampingFees, "CampingFeeId", "Name", regattaVM.RegattaCampingFees.Select(e => e.CampingFeeId).ToList());
            return View(regattaVM);
        }

        // GET: Regatta/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var regatta = await _context.Regattas
                .Include(r => r.Club)
                .SingleOrDefaultAsync(m => m.RegattaId == id);
            if (regatta == null)
            {
                return NotFound();
            }

            return View(regatta);
        }

        // POST: Regatta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var regatta = await _context.Regattas.SingleOrDefaultAsync(m => m.RegattaId == id);
            _context.Regattas.Remove(regatta);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Choose(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var regatta = await _context.Regattas.SingleOrDefaultAsync(m => m.RegattaId == id);
            regatta.Choosen = true;
            _context.Update(regatta);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult CreateProgram(int id)
        {
            var model = _context.Regattas.FirstOrDefault(e => e.RegattaId == id);
            var reportedRaces = _context.ReportedRaces.Include(e => e.Competition).Where(e => e.RegattaId == id);
            var reportedStartboats = _context.ReportedStartboats.Where(e => e.RegattaId == id);
            var reportedStartboatMember = _context.ReportedStartboatMembers;
            var reportedStartboatStandby = _context.ReportedStartboatStandbys;
            var raceDraw = _context.RaceDraws;
            int sbcount = 0;
            int startbahn = 1;            

            foreach(var rr in reportedRaces)
            {
                sbcount = reportedStartboats.Where(e => e.ReportedRaceId == rr.ReportedRaceId).Count();
                if (sbcount > 0)
                {
                    foreach(var rd in raceDraw)
                    {
                        if(sbcount >= rd.ReportedSBCountFrom && sbcount <= rd.ReportedSBCountTo)
                        {
                            for(var vl = 0; vl < rd.VorlaufCount; vl++)
                            {
                                _context.Races.Add(new Race { BoatclassId = rr.Competition.BoatclassId, Gender = rr.Gender, OldclassId = rr.OldclassId, RaceclassId = rr.Competition.RaceclassId, RacestatusId = 1, RaceTypId = 1, RegattaId = id, RaceCode = string.Format("{0}V",rr.RaceCode.Substring(0, 5)) });                                                                
                            }

                            for(var hl = 0; hl < rd.HoffnungslaufCount; hl++)
                            {
                                _context.Races.Add(new Race { BoatclassId = rr.Competition.BoatclassId, Gender = rr.Gender, OldclassId = rr.OldclassId, RaceclassId = rr.Competition.RaceclassId, RacestatusId = 1, RaceTypId = 3, RegattaId = id, RaceCode = string.Format("{0}H", rr.RaceCode.Substring(0, 5)) });                                
                            }

                            for(var zl = 0; zl < rd.ZwischenlaufCount; zl ++)
                            {
                                _context.Races.Add(new Race { BoatclassId = rr.Competition.BoatclassId, Gender = rr.Gender, OldclassId = rr.OldclassId, RaceclassId = rr.Competition.RaceclassId, RacestatusId = 1, RaceTypId = 2, RegattaId = id, RaceCode = string.Format("{0}Z", rr.RaceCode.Substring(0, 5)) });
                            }

                            for(var el = 0; el < rd.EndlaufCount; el++)
                            {
                                _context.Races.Add(new Race { BoatclassId = rr.Competition.BoatclassId, Gender = rr.Gender, OldclassId = rr.OldclassId, RaceclassId = rr.Competition.RaceclassId, RacestatusId = 1, RaceTypId = 4, RegattaId = id, RaceCode = rr.RaceCode });                                
                            }                            
                        }
                    }                    
                }                
            }
            _context.SaveChanges();

            foreach (var newrace in _context.Races.Where(e => e.RegattaId == id))
            {
                foreach(var rrrr in reportedRaces)
                {
                    if (newrace.BoatclassId == rrrr.Competition.BoatclassId && newrace.Gender == rrrr.Gender && newrace.OldclassId == rrrr.OldclassId && newrace.RaceclassId == rrrr.Competition.RaceclassId && newrace.RacestatusId == 1 && newrace.RaceTypId == 1 && newrace.RegattaId == id)
                    {
                        startbahn = 1;
                        for (var i = 1; i <= reportedStartboats.Where(e => e.ReportedRaceId == rrrr.ReportedRaceId).Count(); i++)
                        {
                            if(startbahn == 7)
                            {
                                startbahn = 1;
                            }
                            _context.Startboats.Add(new Startboat { RaceId = newrace.RaceId, RegattaId = id, ClubId = reportedStartboats.FirstOrDefault(e => e.ReportedRaceId == rrrr.ReportedRaceId).ClubId, StartboatstatusId = 6, Startslot = startbahn });
                            startbahn++;
                        }
                    }                    
                }                
            }
            _context.SaveChanges();

            foreach(var nr in _context.Races.Where(e => e.RegattaId == id))
            {            
                foreach(var rrrrrr in reportedRaces.Where(e => e.RegattaId == id))
                {
                    foreach(var newsb in _context.Startboats.Where(e => e.RegattaId == id))
                    {
                        foreach (var rsb in reportedStartboats.Where(e => e.RegattaId == id))
                        {                            
                            foreach (var rsbm in reportedStartboatMember)
                            {
                                if (rsbm.ReportedStartboatId == rsb.ReportedStartboatId && rsb.ReportedRaceId == rrrrrr.ReportedRaceId)
                                {
                                    _context.StartboatMembers.Add(new StartboatMember { StartboatId = newsb.StartboatId, MemberId = rsbm.MemberId, SeatNumber = rsbm.Seatnumber });
                                }
                            }
                            _context.SaveChanges();
                        }
                    }                                       
                }
            }

            return View();
        }

        private bool RegattaExists(int id)
        {
            return _context.Regattas.Any(e => e.RegattaId == id);
        }

        private RegattaVM populateRegattaVM(int? id)
        {
            RegattaVM rvm = new RegattaVM();
            var regatta = _context.Regattas.Include(e => e.Waters).Include(e => e.Club).FirstOrDefault(e => e.RegattaId == id);
            var roc = _context.RegattaOldclasses.Where(e => e.RegattaId == id);
            var rcf = _context.RegattaCampingFees.Where(e => e.RegattaId == id);
            var comp = _context.RegattaCompetitions.Where(e => e.RegattaId == id);
            var rsf = _context.RegattaStartingFees.Where(e => e.RegattaId == id);

            rvm.RegattaId = regatta.RegattaId;
            rvm.RegattaName = regatta.Name;
            rvm.RegattaVon = regatta.FromDate;
            rvm.RegattaBis = regatta.ToDate;
            rvm.Waterdepth = regatta.Waterdepth;
            rvm.Startslots = regatta.Startslots;
            rvm.ReportText = regatta.ReportText;
            rvm.ReportSchedule = regatta.ReportSchedule;
            rvm.ReportOpening = regatta.ReportOpening;
            rvm.ReportAddress = regatta.ReportAddress;
            rvm.ReportTel = regatta.ReportTel;
            rvm.ReportFax = regatta.ReportFax;
            rvm.ReportMail = regatta.ReportMail;            
            rvm.Judge = regatta.Judge;
            rvm.Awards = regatta.Awards;
            rvm.Security = regatta.Security;
            rvm.ScheduleText = regatta.ScheduleText;
            rvm.SubscriberFee = regatta.SubscriberFee;
            rvm.Accomodation = regatta.Accomodation;
            rvm.Comment = regatta.Comment;
            rvm.Catering = regatta.Catering;
            rvm.WaterId = regatta.WaterId;
            rvm.ClubId = regatta.ClubId;

            rvm.RegattaOldclasses = roc;
            rvm.RegattaCampingFees = rcf;
            rvm.RegattaCompetitions = comp;
            rvm.RegattaStartingFees = rsf;

            rvm.Oldclasses = _context.Oldclasses.ToList();
            rvm.CampingFees = _context.CampingFees.ToList();
            rvm.StartingFees = _context.StartingFees.Include(e => e.Boatclasses).Include(e => e.Oldclasses).ToList();
            rvm.Raceclasses = _context.Raceclasses.ToList();
            rvm.Competitions = _context.Competitions.Include(e => e.Boatclasses).Include(e => e.Raceclasses).ToList();

            return rvm;
        }
    }
}
