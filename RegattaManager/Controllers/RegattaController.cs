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
            var reportedRaces = _context.ReportedRaces.Include(e => e.Competition).Where(e => e.RegattaId == id).ToList();
            var reportedStartboats = _context.ReportedStartboats.Where(e => e.RegattaId == id).ToList();;
            var reportedStartboatMember = _context.ReportedStartboatMembers.ToList();
            var reportedStartboatStandby = _context.ReportedStartboatStandbys.ToList();
            var raceDraw = _context.RaceDraws.ToList();
            int sbcount = 0;
            int sbcounter = 0;
            int rrcounter = 0;
            int startbahn = 1;
            List<ReportedStartboat> repsbtemp = new List<ReportedStartboat>();
            List<int> rsb = new List<int>();
            List<int> newrr = new List<int>();

            foreach(var rr in reportedRaces)
            {
                sbcount = reportedStartboats.Where(e => e.ReportedRaceId == rr.ReportedRaceId).Count();
                if (sbcount >= 2)
                {
                    if(rr.StartboatCount != sbcount || rr.isCreated == false)
                    {
                        newrr.Add(rr.ReportedRaceId);
                        foreach (var rd in raceDraw)
                        {
                            if (sbcount >= rd.ReportedSBCountFrom && sbcount <= rd.ReportedSBCountTo && rr.isAbteilungslauf == rd.isAbteilungslauf)
                            {
                                for (var vl = 0; vl < rd.VorlaufCount; vl++)
                                {
                                    _context.Races.Add(new Race { BoatclassId = rr.Competition.BoatclassId, Gender = rr.Gender, OldclassId = rr.OldclassId, RaceclassId = rr.Competition.RaceclassId, ReportedRaceId = rr.ReportedRaceId, RaceDrawId = rd.RaceDrawId, RacestatusId = 1, RaceTypId = 1, RegattaId = id, Comment = rr.Comment, Sequence = vl + 1, RaceCode = string.Format("{0}V{1}", rr.RaceCode.Substring(0, 5), vl + 1) });
                                }

                                for (var hl = 0; hl < rd.HoffnungslaufCount; hl++)
                                {
                                    _context.Races.Add(new Race { BoatclassId = rr.Competition.BoatclassId, Gender = rr.Gender, OldclassId = rr.OldclassId, RaceclassId = rr.Competition.RaceclassId, ReportedRaceId = rr.ReportedRaceId, RaceDrawId = rd.RaceDrawId, RacestatusId = 1003, RaceTypId = 3, RegattaId = id, Comment = rr.Comment, Sequence = hl + 1, RaceCode = string.Format("{0}H{1}", rr.RaceCode.Substring(0, 5), hl + 1) });
                                }

                                for (var zl = 0; zl < rd.ZwischenlaufCount; zl++)
                                {
                                    _context.Races.Add(new Race { BoatclassId = rr.Competition.BoatclassId, Gender = rr.Gender, OldclassId = rr.OldclassId, RaceclassId = rr.Competition.RaceclassId, ReportedRaceId = rr.ReportedRaceId, RaceDrawId = rd.RaceDrawId, RacestatusId = 1003, RaceTypId = 2, RegattaId = id, Comment = rr.Comment, Sequence = zl + 1, RaceCode = string.Format("{0}Z{1}", rr.RaceCode.Substring(0, 5), zl + 1) });
                                }

                                for (var el = 0; el < rd.EndlaufCount; el++)
                                {
                                    if (sbcount <= model.Startslots)
                                    {
                                        _context.Races.Add(new Race { BoatclassId = rr.Competition.BoatclassId, Gender = rr.Gender, OldclassId = rr.OldclassId, RaceclassId = rr.Competition.RaceclassId, ReportedRaceId = rr.ReportedRaceId, RaceDrawId = rd.RaceDrawId, RacestatusId = 1, RaceTypId = 4, RegattaId = id, Sequence = 1, RaceCode = rr.RaceCode, Comment = rr.Comment });
                                    }
                                    else
                                    {
                                        _context.Races.Add(new Race { BoatclassId = rr.Competition.BoatclassId, Gender = rr.Gender, OldclassId = rr.OldclassId, RaceclassId = rr.Competition.RaceclassId, ReportedRaceId = rr.ReportedRaceId, RaceDrawId = rd.RaceDrawId, RacestatusId = 1003, RaceTypId = 4, RegattaId = id, Sequence = 1, RaceCode = rr.RaceCode, Comment = rr.Comment });
                                    }
                                }
                            }
                        }

                        rr.isCreated = true;
                        rr.StartboatCount = sbcount;
                        rr.modifiedDate = DateTime.Now;
                        _context.ReportedRaces.Update(rr);
                    }                                        
                }
                else
                {
                    if(!_context.Races.Any(e => e.ReportedRaceId == rr.ReportedRaceId))
                    {
                        newrr.Add(rr.ReportedRaceId);

                        _context.Races.Add(new Race { BoatclassId = rr.Competition.BoatclassId, Gender = rr.Gender, OldclassId = rr.OldclassId, RaceclassId = rr.Competition.RaceclassId, ReportedRaceId = rr.ReportedRaceId, RaceDrawId = 0, RacestatusId = 1006, RaceTypId = 4, RegattaId = id, Sequence = 1, RaceCode = rr.RaceCode, Comment = rr.Comment });

                        rr.isCreated = false;
                        rr.StartboatCount = sbcount;
                        rr.modifiedDate = DateTime.Now;
                        _context.ReportedRaces.Update(rr);
                    }                                        
                }
            }
            _context.SaveChanges();

            var newraces = _context.ReportedRaces.Where(e => newrr.Contains(e.ReportedRaceId)).ToList();

            foreach (var rrrr in newraces)
            {
                rrcounter = _context.Races.Where(e => e.ReportedRaceId == rrrr.ReportedRaceId && e.RaceTypId == 1).Count();
                sbcount = reportedStartboats.Where(e => e.ReportedRaceId == rrrr.ReportedRaceId).Count();
                sbcounter = sbcount;
                
                foreach (var newrace in _context.Races.Where(e => e.ReportedRaceId == rrrr.ReportedRaceId).OrderBy(e => e.RaceCode))
                {
                    if (sbcount <= model.Startslots)
                    {
                        if (newrace.ReportedRaceId == rrrr.ReportedRaceId && newrace.RaceTypId == 4)
                        {
                            repsbtemp = reportedStartboats.Where(e => e.ReportedRaceId == rrrr.ReportedRaceId).OrderBy(e => Guid.NewGuid()).ToList();
                            startbahn = 1;
                            foreach (var tempsb in repsbtemp)
                            {
                                if (!rsb.Contains(tempsb.ReportedStartboatId))
                                {
                                    _context.Startboats.Add(new Startboat { RaceId = newrace.RaceId, RegattaId = id, ClubId = tempsb.ClubId, ReportedStartboatId = tempsb.ReportedStartboatId, StartboatstatusId = 6, Startslot = startbahn });
                                }
                                rsb.Add(tempsb.ReportedStartboatId);
                                startbahn++;
                            }
                        }
                    }
                    else
                    {
                        if (newrace.ReportedRaceId == rrrr.ReportedRaceId && newrace.RaceTypId == 1)
                        {
                            repsbtemp = reportedStartboats.Where(e => e.ReportedRaceId == rrrr.ReportedRaceId).OrderBy(e => Guid.NewGuid()).ToList();
                            startbahn = 1;
                            foreach (var tempsb in repsbtemp)
                            {
                                if (!rsb.Contains(tempsb.ReportedStartboatId) && startbahn <= model.Startslots && rrcounter > 1 && sbcounter > 4)
                                {
                                    _context.Startboats.Add(new Startboat { RaceId = newrace.RaceId, RegattaId = id, ClubId = tempsb.ClubId, ReportedStartboatId = tempsb.ReportedStartboatId, StartboatstatusId = 6, Startslot = startbahn, Gender = tempsb.Gender });
                                    rsb.Add(tempsb.ReportedStartboatId);
                                    startbahn++;
                                    sbcounter--;
                                }
                                else if (!rsb.Contains(tempsb.ReportedStartboatId) && startbahn <= model.Startslots && rrcounter == 1)
                                {
                                    _context.Startboats.Add(new Startboat { RaceId = newrace.RaceId, RegattaId = id, ClubId = tempsb.ClubId, ReportedStartboatId = tempsb.ReportedStartboatId, StartboatstatusId = 6, Startslot = startbahn, Gender = tempsb.Gender });
                                    rsb.Add(tempsb.ReportedStartboatId);
                                    startbahn++;
                                    sbcounter--;
                                }
                            }
                            rrcounter--;
                        }
                    }
                }                               
            }
            _context.SaveChanges();

            foreach(var nr in _context.Races.Where(e => e.RegattaId == id))
            {            
                foreach(var rr in reportedRaces)
                {                    
                    if (nr.ReportedRaceId == rr.ReportedRaceId)
                    {
                        foreach(var newsb in _context.Startboats.Where(e => e.RegattaId == id && e.RaceId == nr.RaceId))
                        {
                            foreach (var rsbb in reportedStartboats.Where(e => e.RegattaId == id && e.ReportedRaceId == rr.ReportedRaceId).OrderBy(e => e.Gender).ThenBy(e => e.ClubId))
                            {        
                                if(newsb.ReportedStartboatId == rsbb.ReportedStartboatId)       
                                {
                                    foreach (var rsbm in reportedStartboatMember.Where(e => e.ReportedStartboatId == rsbb.ReportedStartboatId))
                                    {
                                        _context.StartboatMembers.Add(new StartboatMember { StartboatId = newsb.StartboatId, MemberId = rsbm.MemberId, SeatNumber = rsbm.Seatnumber });                                        
                                    }
                                    
                                    foreach (var rsbs in reportedStartboatStandby.Where(e => e.ReportedStartboatId == rsbb.ReportedStartboatId))
                                    {
                                        _context.StartboatStandbys.Add(new StartboatStandby { StartboatId = newsb.StartboatId, MemberId = rsbs.MemberId, Standbynumber = rsbs.Standbynumber });                                        
                                    }
                                }                                                                             
                            }
                        } 
                    }                                                          
                }
            }
            _context.SaveChanges();

            return RedirectToAction("Index", "Race");
        }

        public IActionResult UpdateProgram(int id)
        {
            var Startboats = _context.Startboats.Where(e => e.RegattaId == id).ToList();
            var newReportedStartboats = _context.ReportedStartboats.Where(e => !Startboats.Select(i => i.ReportedStartboatId).Contains(e.ReportedStartboatId)).ToList();
            var Races = _context.Races.Where(e => e.RacestatusId == 1).ToList();
            List<int> startslots = new List<int>();
            List<int> addedstartslots = new List<int>();
            List<int> rsb = new List<int>();

            foreach (var r in Races)
            {
                startslots = getFreeStartslot(r.RaceId);
                addedstartslots.Clear();
                if (startslots != null)
                {                    
                    foreach(var nrsb in newReportedStartboats)
                    {                          
                        foreach(var sl in startslots)
                        {
                            if (nrsb.ReportedRaceId == r.ReportedRaceId && !rsb.Contains(nrsb.ReportedStartboatId) && nrsb.NoStartslot == false && !addedstartslots.Contains(sl))
                            {
                                _context.Startboats.Add(new Startboat { RaceId = r.RaceId, RegattaId = nrsb.RegattaId, ClubId = nrsb.ClubId, ReportedStartboatId = nrsb.ReportedStartboatId, StartboatstatusId = 6, Startslot = sl, Gender = nrsb.Gender });
                                rsb.Add(nrsb.ReportedStartboatId);
                                addedstartslots.Add(sl);
                            }                                
                        }
                    }   
                    
                }
            }
            _context.SaveChanges();

            var newStartboats = _context.Startboats.Where(e => newReportedStartboats.Select(i => i.ReportedStartboatId).Contains(e.ReportedStartboatId)).ToList();
            var newReportedSBM = _context.ReportedStartboatMembers.Where(e => newReportedStartboats.Select(i => i.ReportedStartboatId).Contains(e.ReportedStartboatId)).ToList();
            var newReportedSBS = _context.ReportedStartboatStandbys.Where(e => newReportedStartboats.Select(i => i.ReportedStartboatId).Contains(e.ReportedStartboatId)).ToList();
            List<ReportedStartboatMember> addedSBM = new List<ReportedStartboatMember>();
            List<ReportedStartboatStandby> addedSBS = new List<ReportedStartboatStandby>();

            foreach (var nsb in newStartboats)
            {
                foreach(var nrsbm in newReportedSBM)
                {
                    if(nrsbm.ReportedStartboatId == nsb.ReportedStartboatId && !addedSBM.Contains(nrsbm))
                    {
                        _context.StartboatMembers.Add(new StartboatMember { MemberId = nrsbm.MemberId, StartboatId = nsb.StartboatId, SeatNumber = nrsbm.Seatnumber });
                        addedSBM.Add(nrsbm);
                    }
                }
                foreach(var nrsbs in newReportedSBS)
                {
                    if(nrsbs.ReportedStartboatId == nsb.ReportedStartboatId && !addedSBS.Contains(nrsbs))
                    {
                        _context.StartboatStandbys.Add(new StartboatStandby { MemberId = nrsbs.MemberId, StartboatId = nsb.StartboatId, Standbynumber = nrsbs.Standbynumber });
                        addedSBS.Add(nrsbs);
                    }
                }
            }
            _context.SaveChanges();
            
            return RedirectToAction("Index", "Race");
        }

        private List<int> getFreeStartslot(int raceid)
        {
            int maxStartslot = _context.Startboats.Where(e => e.RaceId == raceid).Select(e => e.Startslot).Max();
            List<int> freeslots = new List<int>();

            if(maxStartslot < 6)
            {
                maxStartslot = 6;
            }

            for(var i=1;i<=maxStartslot;i++)
            {
                if(!_context.Startboats.Any(e => e.RaceId == raceid && e.Startslot == i))
                {
                    freeslots.Add(i);
                }
            }

            return freeslots;
        }

        [HttpGet]
        public IActionResult CreateStarttimes()
        {
            var regatta = _context.Regattas.Where(e => e.Choosen == true).FirstOrDefault();
            var d1 = new DateTime(0001, 1, 1, 0, 0, 0);
            var races = _context.Races.Include(e => e.Oldclass).Include(e => e.Racestatus).Include(e => e.Raceclass).Include(e => e.Boatclass).Where(e => e.Racestatus.Name != "zu wenig Teilnehmer" && e.Starttime == d1).OrderBy(e => e.Oldclass.ToAge).ToList();

            ViewBag.configuredRaces = _context.Races.Include(e => e.Oldclass).Include(e => e.Racestatus).Include(e => e.Raceclass).Include(e => e.Boatclass).Where(e => e.Racestatus.Name != "zu wenig Teilnehmer" && e.Starttime > d1 && e.Starttime <= regatta.ToDate).OrderBy(e => e.Starttime).ToList();
            ViewBag.configuredRacesDayTwo = _context.Races.Include(e => e.Oldclass).Include(e => e.Racestatus).Include(e => e.Raceclass).Include(e => e.Boatclass).Where(e => e.Racestatus.Name != "zu wenig Teilnehmer" && e.Starttime > regatta.ToDate).OrderBy(e => e.Starttime).ToList();

            ViewBag.starttime = _context.Races.Where(e => e.Starttime >= regatta.FromDate && e.Starttime <= regatta.ToDate).OrderByDescending(e => e.Starttime).First().Starttime;
            ViewBag.starttimeDayTwo = _context.Races.Where(e => e.Starttime >= regatta.ToDate).OrderByDescending(e => e.Starttime).First().Starttime;
            ViewBag.minutestep = 2;

            return View(races);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfigureRace(int raceId, int minutestep)
        {
            var regatta = _context.Regattas.Where(e => e.Choosen == true).FirstOrDefault();
            var race = _context.Races.SingleOrDefault(e => e.RaceId == raceId);
            var starttime = _context.Races.Where(e => e.Starttime >= regatta.FromDate && e.Starttime <= regatta.ToDate).OrderByDescending(e => e.Starttime).First().Starttime;

            starttime = starttime.AddMinutes(minutestep);

            race.Starttime = starttime;

            _context.Entry(race).State = EntityState.Modified;
            _context.SaveChanges();

            return RedirectToAction("CreateStarttimes", "Regatta");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfigureRaceDayTwo(int raceId, int minutestep)
        {
            var regatta = _context.Regattas.Where(e => e.Choosen == true).FirstOrDefault();
            var race = _context.Races.SingleOrDefault(e => e.RaceId == raceId);
            var starttimeDayTwo = _context.Races.Where(e => e.Starttime >= regatta.ToDate).OrderByDescending(e => e.Starttime).First().Starttime;

            starttimeDayTwo = starttimeDayTwo.AddMinutes(minutestep);

            race.Starttime = starttimeDayTwo;

            _context.Entry(race).State = EntityState.Modified;
            _context.SaveChanges();

            return RedirectToAction("CreateStarttimes", "Regatta");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UnconfigureRace(int raceId)
        {
            var race = _context.Races.SingleOrDefault(e => e.RaceId == raceId);
            var starttime = new DateTime(0001, 1, 1, 0, 0, 0);

            race.Starttime = starttime;

            _context.Entry(race).State = EntityState.Modified;
            _context.SaveChanges();

            return RedirectToAction("CreateStarttimes", "Regatta");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateRaceStarttime(int raceId, DateTime starttime)
        {
            var race = _context.Races.SingleOrDefault(e => e.RaceId == raceId);
            
            race.Starttime = starttime;

            _context.Entry(race).State = EntityState.Modified;
            _context.SaveChanges();

            return RedirectToAction("CreateStarttimes", "Regatta");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AllRacesDown(int raceId, int minutestep)
        {
            var currentrace = _context.Races.SingleOrDefault(e => e.RaceId == raceId);

            if(currentrace != null)
            {
                var racestomove = _context.Races.Where(e => e.Starttime >= currentrace.Starttime).ToList();

                foreach(var r in racestomove)
                {
                    r.Starttime = r.Starttime.AddMinutes(minutestep);
                    _context.Entry(r).State = EntityState.Modified;
                }

                _context.SaveChanges();
            }

            return RedirectToAction("CreateStarttimes", "Regatta");
        }

        public IActionResult SetRaceTimes(int id, int minutestep, DateTime day1start, DateTime day2start)
        {
            Regatta regatta = _context.Regattas.Where(x => x.Choosen == true).FirstOrDefault(e => e.RegattaId == id);

            if(regatta != null)
            {
                DateTime globaltimestamp = day1start;
                var d1 = new DateTime(0001, 1, 1, 0, 0, 0);

                int auszeitRennen = 40 / minutestep;

                List<Race> vorlaeufe = _context.Races.Include(x => x.Oldclass).Include(x => x.RaceTyp).Include(x => x.Racestatus).Where(e => e.RaceTyp.Name == "Vorlauf" && e.Racestatus.Name != "zu wenig Teilnehmer").OrderBy(e => e.RaceCode).ToList();
                List<Race> zwischenlaeufe = _context.Races.Include(x => x.Oldclass).Include(x => x.RaceTyp).Include(x => x.Racestatus).Where(e => e.RaceTyp.Name == "Zwischenlauf" && e.Racestatus.Name != "zu wenig Teilnehmer").OrderBy(e => e.RaceCode).ToList();
                List<Race> hoffnungslaeufe = _context.Races.Include(x => x.Oldclass).Include(x => x.RaceTyp).Include(x => x.Racestatus).Where(e => e.RaceTyp.Name == "Hoffnungslauf" && e.Racestatus.Name != "zu wenig Teilnehmer").OrderBy(e => e.RaceCode).ToList();

                List<Race> availableRaces = new List<Race>();

                List<Startboat> startboats = _context.Startboats.ToList();
                List<StartboatMember> startboatMembers = _context.StartboatMembers.ToList();

                List<int> previousRaceIds = new List<int>();
                List<int> previousStartboatIds = new List<int>();
                List<int> previousMemberIds = new List<int>();
                List<int> currentStartboatIds = new List<int>();
                List<int> currentMemberIds = new List<int>();

                Race firstrace = vorlaeufe.Where(e => e.Oldclass.FromAge >= 13).OrderBy(e => e.Oldclass.FromAge).FirstOrDefault();

                firstrace.Starttime = day1start;
                _context.Races.Update(firstrace);
                _context.SaveChanges();

                vorlaeufe = _context.Races.Include(x => x.Oldclass).Include(x => x.RaceTyp).Include(x => x.Racestatus).Where(e => e.RaceTyp.Name == "Vorlauf" && e.Racestatus.Name != "zu wenig Teilnehmer" && e.RaceId != firstrace.RaceId).OrderBy(e => e.RaceCode).ToList();



                foreach(var v in vorlaeufe)
                {                    
                    previousRaceIds = vorlaeufe.Where(e => e.Starttime <= globaltimestamp && e.Starttime != d1).OrderByDescending(e => e.Starttime).Select(e => e.RaceId).Take(auszeitRennen).ToList();
                    previousStartboatIds = startboats.Where(e => previousRaceIds.Contains(e.RaceId)).Select(e => e.StartboatId).ToList();
                    previousMemberIds = startboatMembers.Where(e => previousStartboatIds.Contains(e.StartboatId)).Select(e => e.MemberId).ToList();

                    currentStartboatIds = startboats.Where(e => e.RaceId == v.RaceId).Select(e => e.StartboatId).ToList();
                    currentMemberIds = startboatMembers.Where(e => currentStartboatIds.Contains(e.StartboatId)).Select(e => e.MemberId).ToList();

                    if(!previousMemberIds.Except(currentMemberIds).Any())
                    {
                        globaltimestamp = globaltimestamp.AddMinutes(minutestep);
                        v.Starttime = globaltimestamp;
                        _context.Races.Update(v);
                    }
                    else
                    {
                        availableRaces.Add(v);
                    }
                }
                foreach(var h in hoffnungslaeufe)
                {
                    previousRaceIds = vorlaeufe.Where(e => e.Starttime <= globaltimestamp && e.Starttime != d1).OrderByDescending(e => e.Starttime).Select(e => e.RaceId).Take(auszeitRennen).ToList();
                    previousStartboatIds = startboats.Where(e => previousRaceIds.Contains(e.RaceId)).Select(e => e.StartboatId).ToList();
                    previousMemberIds = startboatMembers.Where(e => previousStartboatIds.Contains(e.StartboatId)).Select(e => e.MemberId).ToList();

                    currentStartboatIds = startboats.Where(e => e.RaceId == h.RaceId).Select(e => e.StartboatId).ToList();
                    currentMemberIds = startboatMembers.Where(e => currentStartboatIds.Contains(e.StartboatId)).Select(e => e.MemberId).ToList();

                    if (!previousMemberIds.Except(currentMemberIds).Any())
                    {
                        globaltimestamp = globaltimestamp.AddMinutes(minutestep);
                        h.Starttime = globaltimestamp;
                        _context.Races.Update(h);
                    }
                    else
                    {
                        availableRaces.Add(h);
                    }
                }
                foreach(var z in zwischenlaeufe)
                {
                    previousRaceIds = vorlaeufe.Where(e => e.Starttime <= globaltimestamp && e.Starttime != d1).OrderByDescending(e => e.Starttime).Select(e => e.RaceId).Take(auszeitRennen).ToList();
                    previousStartboatIds = startboats.Where(e => previousRaceIds.Contains(e.RaceId)).Select(e => e.StartboatId).ToList();
                    previousMemberIds = startboatMembers.Where(e => previousStartboatIds.Contains(e.StartboatId)).Select(e => e.MemberId).ToList();

                    currentStartboatIds = startboats.Where(e => e.RaceId == z.RaceId).Select(e => e.StartboatId).ToList();
                    currentMemberIds = startboatMembers.Where(e => currentStartboatIds.Contains(e.StartboatId)).Select(e => e.MemberId).ToList();

                    if (!previousMemberIds.Except(currentMemberIds).Any())
                    {
                        globaltimestamp = globaltimestamp.AddMinutes(minutestep);
                        h.Starttime = globaltimestamp;
                        _context.Races.Update(z);
                    }
                    else
                    {
                        availableRaces.Add(z);
                    }
                }
            }

            return RedirectToAction("Index","Race");
        }

        private DateTime SetTimes(List<Race> races, DateTime timestamp)
        {
            foreach (var r in races)
            {
                r.Starttime = timestamp;
                _context.Races.Update(r);
                if (r.BoatclassId == 10)
                {                                        
                    timestamp = timestamp.AddMinutes(60);
                }
                else
                {
                    if(r.BoatclassId == 7 || r.BoatclassId == 8 || r.BoatclassId == 9 || r.BoatclassId == 10 || r.BoatclassId == 11)
                    {
                        timestamp = timestamp.AddMinutes(6);
                    }
                    else
                    {
                        timestamp = timestamp.AddMinutes(3);
                    }                    
                }
            }
            _context.SaveChanges();

            return timestamp;
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
            rvm.StartingFees = _context.StartingFees.Include(e => e.Boatclasses).ToList();
            rvm.Raceclasses = _context.Raceclasses.ToList();
            rvm.Competitions = _context.Competitions.Include(e => e.Boatclasses).Include(e => e.Raceclasses).ToList();

            return rvm;
        }        
    }
}
