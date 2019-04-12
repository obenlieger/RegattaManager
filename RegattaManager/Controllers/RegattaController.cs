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

        public IActionResult SetRaceTimes(int id)
        {
            var regatta = _context.Regattas.FirstOrDefault(e => e.RegattaId == id);

            if(regatta != null && regatta.Name.Contains("Sprintpokal"))
            {
                DateTime globaltimestamp = regatta.FromDate.AddDays(2);
                globaltimestamp = globaltimestamp.AddHours(2);

                var nr1 = _context.Races.Where(e => e.Boatclass.Name == "K1" && e.OldclassId == 1010 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr2 = _context.Races.Where(e => e.Boatclass.Name == "K1" && e.OldclassId == 1011 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr3 = _context.Races.Where(e => e.Boatclass.Name == "K2" && e.OldclassId == 3 && e.Gender == "W" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr4 = _context.Races.Where(e => e.Boatclass.Name == "K4" && e.OldclassId == 5 && e.Gender == "X" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr5 = _context.Races.Where(e => e.Boatclass.Name == "K2" && e.OldclassId == 7 && e.Gender == "X" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr6 = _context.Races.Where(e => e.Boatclass.Name == "S4" && e.OldclassId == 2 && e.Gender == "M" && e.Raceclass.Length == 100 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr7 = _context.Races.Where(e => e.Boatclass.Name == "K4" && e.OldclassId == 2 && e.Gender == "W" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr8 = _context.Races.Where(e => e.Boatclass.Name == "K1" && e.OldclassId == 1004 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr9 = _context.Races.Where(e => e.Boatclass.Name == "K1" && e.OldclassId == 1004 && e.Gender == "W" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr10 = _context.Races.Where(e => e.Boatclass.Name == "K2" && e.OldclassId == 4 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr11 = _context.Races.Where(e => e.Boatclass.Name == "K1" && e.OldclassId == 1002 && e.Gender == "W" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr12 = _context.Races.Where(e => e.Boatclass.Name == "K1" && e.OldclassId == 1003 && e.Gender == "W" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr13 = _context.Races.Where(e => e.Boatclass.Name == "C2" && e.OldclassId == 4 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr14 = _context.Races.Where(e => e.Boatclass.Name == "C1" && e.OldclassId == 4 && e.Gender == "W" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr15 = _context.Races.Where(e => e.Boatclass.Name == "K4" && e.OldclassId == 6 && e.Gender == "X" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr16 = _context.Races.Where(e => e.Boatclass.Name == "K2" && e.Oldclass.FromAge >= 32 && e.Oldclass.ToAge <= 99 && e.Gender == "X" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr17 = _context.Races.Where(e => e.Boatclass.Name == "S4" && e.OldclassId == 3 && e.Gender == "M" && e.Raceclass.Length == 100 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr18 = _context.Races.Where(e => e.Boatclass.Name == "K4" && e.OldclassId == 3 && e.Gender == "W" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr19 = _context.Races.Where(e => e.Boatclass.Name == "K1" && e.OldclassId == 5 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr20 = _context.Races.Where(e => e.Boatclass.Name == "K2" && e.OldclassId == 5 && e.Gender == "W" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr21 = _context.Races.Where(e => e.Boatclass.Name == "C1" && e.OldclassId == 5 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr22 = _context.Races.Where(e => e.Boatclass.Name == "C2" && e.OldclassId == 5 && e.Gender == "W" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr23 = _context.Races.Where(e => e.Boatclass.Name == "K4" && e.OldclassId == 7 && e.Gender == "X" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr24 = _context.Races.Where(e => e.Boatclass.Name == "K2" && e.OldclassId == 2 && e.Gender == "X" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr25 = _context.Races.Where(e => e.Boatclass.Name == "K4" && e.OldclassId == 4 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr26 = _context.Races.Where(e => e.Boatclass.Name == "S4" && e.OldclassId == 4 && e.Gender == "W" && e.Raceclass.Length == 100 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr27 = _context.Races.Where(e => e.Boatclass.Name == "K1" && e.OldclassId == 6 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr28 = _context.Races.Where(e => e.Boatclass.Name == "K2" && e.OldclassId == 6 && e.Gender == "W" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr29 = _context.Races.Where(e => e.Boatclass.Name == "K2" && e.OldclassId == 1004 && e.Gender == "X" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr30 = _context.Races.Where(e => e.Boatclass.Name == "C1" && e.OldclassId == 6 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr31 = _context.Races.Where(e => e.Boatclass.Name == "C2" && e.OldclassId == 6 && e.Gender == "W" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr32 = _context.Races.Where(e => e.Boatclass.Name == "S4" && e.OldclassId == 8 && e.Gender == "M" && e.Raceclass.Length == 100 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr33 = _context.Races.Where(e => e.Boatclass.Name == "K4" && e.OldclassId == 8 && e.Gender == "W" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr34 = _context.Races.Where(e => e.Boatclass.Name == "K2" && e.OldclassId == 3 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr35 = _context.Races.Where(e => e.Boatclass.Name == "K1" && e.OldclassId == 1010 && e.Gender == "W" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr36 = _context.Races.Where(e => e.Boatclass.Name == "K1" && e.OldclassId == 1011 && e.Gender == "W" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr37 = _context.Races.Where(e => e.Boatclass.Name == "K1" && e.OldclassId == 1008 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr38 = _context.Races.Where(e => e.Boatclass.Name == "K1" && e.OldclassId == 1009 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr39 = _context.Races.Where(e => e.Boatclass.Name == "K2" && e.OldclassId == 5 && e.Gender == "X" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr40 = _context.Races.Where(e => e.Boatclass.Name == "S4" && e.OldclassId == 7 && e.Gender == "M" && e.Raceclass.Length == 100 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr41 = _context.Races.Where(e => e.Boatclass.Name == "K4" && e.OldclassId == 7 && e.Gender == "W" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr42 = _context.Races.Where(e => e.Boatclass.Name == "S4" && e.OldclassId == 1004 && e.Gender == "M" && e.Raceclass.Length == 100 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr43 = _context.Races.Where(e => e.Boatclass.Name == "K4" && e.OldclassId == 1004 && e.Gender == "W" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr44 = _context.Races.Where(e => e.Boatclass.Name == "K1" && e.OldclassId == 1002 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr48 = _context.Races.Where(e => e.Boatclass.Name == "K1" && e.OldclassId == 1003 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr49 = _context.Races.Where(e => e.Boatclass.Name == "K2" && e.OldclassId == 4 && e.Gender == "W" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr50 = _context.Races.Where(e => e.Boatclass.Name == "C1" && e.OldclassId == 1002 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr51 = _context.Races.Where(e => e.Boatclass.Name == "C1" && e.OldclassId == 1003 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr52 = _context.Races.Where(e => e.Boatclass.Name == "C2" && e.OldclassId == 4 && e.Gender == "W" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr53 = _context.Races.Where(e => e.Boatclass.Name == "K2" && e.OldclassId == 6 && e.Gender == "X" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr54 = _context.Races.Where(e => e.Boatclass.Name == "K4" && e.Oldclass.FromAge >= 32 && e.Oldclass.ToAge <= 99 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr55 = _context.Races.Where(e => e.Boatclass.Name == "S4" && e.Oldclass.FromAge >= 32 && e.Oldclass.ToAge <= 99 && e.Gender == "W" && e.Raceclass.Length == 100 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr56 = _context.Races.Where(e => e.Boatclass.Name == "K1" && e.OldclassId == 1008 && e.Gender == "W" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr57 = _context.Races.Where(e => e.Boatclass.Name == "K1" && e.OldclassId == 1009 && e.Gender == "W" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr58 = _context.Races.Where(e => e.Boatclass.Name == "K4" && e.OldclassId == 5 && e.Gender == "W" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr59 = _context.Races.Where(e => e.Boatclass.Name == "S4" && e.OldclassId == 5 && e.Gender == "M" && e.Raceclass.Length == 100 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr60 = _context.Races.Where(e => e.Boatclass.Name == "K4" && e.OldclassId == 3 && e.Gender == "X" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr61 = _context.Races.Where(e => e.Boatclass.Name == "K1" && e.OldclassId == 7 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr62 = _context.Races.Where(e => e.Boatclass.Name == "K2" && e.OldclassId == 7 && e.Gender == "W" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr63 = _context.Races.Where(e => e.Boatclass.Name == "C1" && e.OldclassId == 7 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr64 = _context.Races.Where(e => e.Boatclass.Name == "C2" && e.OldclassId == 7 && e.Gender == "W" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr65 = _context.Races.Where(e => e.Boatclass.Name == "C4" && e.Oldclass.FromAge >= 13 && e.Oldclass.ToAge <= 16 && e.Gender == "X" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr66 = _context.Races.Where(e => e.Boatclass.Name == "K4" && e.OldclassId == 2 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr67 = _context.Races.Where(e => e.Boatclass.Name == "S4" && e.OldclassId == 2 && e.Gender == "W" && e.Raceclass.Length == 100 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr68 = _context.Races.Where(e => e.Boatclass.Name == "K4" && e.OldclassId == 4 && e.Gender == "X" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr69 = _context.Races.Where(e => e.Boatclass.Name == "S4" && e.OldclassId == 6 && e.Gender == "M" && e.Raceclass.Length == 100 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr70 = _context.Races.Where(e => e.Boatclass.Name == "K4" && e.OldclassId == 6 && e.Gender == "W" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr71 = _context.Races.Where(e => e.Boatclass.Name == "K1" && e.OldclassId == 8 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr72 = _context.Races.Where(e => e.Boatclass.Name == "K1" && e.OldclassId == 10 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr73 = _context.Races.Where(e => e.Boatclass.Name == "K2" && e.Oldclass.FromAge >= 32 && e.Oldclass.ToAge <= 99 && e.Gender == "W" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr74 = _context.Races.Where(e => e.Boatclass.Name == "C1" && e.Oldclass.FromAge >= 32 && e.Oldclass.ToAge <= 99 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr75 = _context.Races.Where(e => e.Boatclass.Name == "C2" && e.Oldclass.FromAge >= 32 && e.Oldclass.ToAge <= 99 && e.Gender == "W" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr76 = _context.Races.Where(e => e.Boatclass.Name == "K2" && e.OldclassId == 2 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr77 = _context.Races.Where(e => e.Boatclass.Name == "K2" && e.OldclassId == 1004 && e.Gender == "W" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr78 = _context.Races.Where(e => e.Boatclass.Name == "K1" && e.OldclassId == 11 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr79 = _context.Races.Where(e => e.Boatclass.Name == "K1" && e.OldclassId == 9 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr80 = _context.Races.Where(e => e.Boatclass.Name == "S8" && e.Oldclass.FromAge >= 0 && e.Oldclass.ToAge <= 99 && e.Gender == "X" && e.Raceclass.Length == 100 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr81 = _context.Races.Where(e => e.Boatclass.Name == "C4" && e.Oldclass.FromAge >= 17 && e.Oldclass.ToAge <= 99 && e.Gender == "X" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr82 = _context.Races.Where(e => e.Boatclass.Name == "K2" && e.OldclassId == 2 && e.Gender == "W" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr83 = _context.Races.Where(e => e.Boatclass.Name == "K2" && e.OldclassId == 1004 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr84 = _context.Races.Where(e => e.Boatclass.Name == "K2" && e.OldclassId == 4 && e.Gender == "X" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr85 = _context.Races.Where(e => e.Boatclass.Name == "K4" && e.OldclassId == 6 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr86 = _context.Races.Where(e => e.Boatclass.Name == "S4" && e.OldclassId == 6 && e.Gender == "W" && e.Raceclass.Length == 100 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr87 = _context.Races.Where(e => e.Boatclass.Name == "K4" && e.Oldclass.FromAge >= 32 && e.Oldclass.ToAge <= 99 && e.Gender == "X" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr88 = _context.Races.Where(e => e.Boatclass.Name == "K2" && e.OldclassId == 3 && e.Gender == "X" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr89 = _context.Races.Where(e => e.Boatclass.Name == "K4" && e.OldclassId == 5 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr90 = _context.Races.Where(e => e.Boatclass.Name == "S4" && e.OldclassId == 5 && e.Gender == "W" && e.Raceclass.Length == 100 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr91 = _context.Races.Where(e => e.Boatclass.Name == "K4" && (e.OldclassId == 2 || e.OldclassId == 1004) && e.Gender == "X" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr92 = _context.Races.Where(e => e.Boatclass.Name == "K2" && e.OldclassId == 7 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr93 = _context.Races.Where(e => e.Boatclass.Name == "K1" && e.OldclassId == 7 && e.Gender == "W" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr94 = _context.Races.Where(e => e.Boatclass.Name == "C2" && e.OldclassId == 7 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr95 = _context.Races.Where(e => e.Boatclass.Name == "C1" && e.OldclassId == 7 && e.Gender == "W" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr96 = _context.Races.Where(e => e.Boatclass.Name == "S4" && e.OldclassId == 3 && e.Gender == "W" && e.Raceclass.Length == 100 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr97 = _context.Races.Where(e => e.Boatclass.Name == "K4" && e.OldclassId == 3 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr98 = _context.Races.Where(e => e.Boatclass.Name == "K2" && e.OldclassId == 5 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr99 = _context.Races.Where(e => e.Boatclass.Name == "K1" && e.OldclassId == 5 && e.Gender == "W" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr100 = _context.Races.Where(e => e.Boatclass.Name == "C2" && e.OldclassId == 5 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr101 = _context.Races.Where(e => e.Boatclass.Name == "C1" && e.OldclassId == 5 && e.Gender == "W" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr102 = _context.Races.Where(e => e.Boatclass.Name == "K2" && e.Oldclass.FromAge >= 32 && e.Oldclass.ToAge <= 49 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr103 = _context.Races.Where(e => e.Boatclass.Name == "K1" && e.Oldclass.FromAge >= 32 && e.Oldclass.ToAge <= 99 && e.Gender == "W" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr104 = _context.Races.Where(e => e.Boatclass.Name == "C2" && e.Oldclass.FromAge >= 32 && e.Oldclass.ToAge <= 99 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr105 = _context.Races.Where(e => e.Boatclass.Name == "C1" && e.Oldclass.FromAge >= 32 && e.Oldclass.ToAge <= 99 && e.Gender == "W" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr109 = _context.Races.Where(e => e.Boatclass.Name == "K2" && e.Oldclass.FromAge >= 50 && e.Oldclass.ToAge <= 99 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr110 = _context.Races.Where(e => e.Boatclass.Name == "S4" && e.OldclassId == 4 && e.Gender == "M" && e.Raceclass.Length == 100 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr111 = _context.Races.Where(e => e.Boatclass.Name == "K4" && e.OldclassId == 4 && e.Gender == "W" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr112 = _context.Races.Where(e => e.Boatclass.Name == "K2" && e.OldclassId == 6 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr113 = _context.Races.Where(e => e.Boatclass.Name == "K1" && e.OldclassId == 6 && e.Gender == "W" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr114 = _context.Races.Where(e => e.Boatclass.Name == "C2" && e.OldclassId == 6 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr115 = _context.Races.Where(e => e.Boatclass.Name == "C1" && e.OldclassId == 6 && e.Gender == "W" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr116 = _context.Races.Where(e => e.Boatclass.Name == "S4" && e.OldclassId == 1004 && e.Gender == "W" && e.Raceclass.Length == 100 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr117 = _context.Races.Where(e => e.Boatclass.Name == "K4" && e.OldclassId == 1004 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr118 = _context.Races.Where(e => e.Boatclass.Name == "S4" && e.OldclassId == 7 && e.Gender == "W" && e.Raceclass.Length == 100 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();
                var nr119 = _context.Races.Where(e => e.Boatclass.Name == "K4" && e.OldclassId == 7 && e.Gender == "M" && e.Raceclass.Length == 250 && e.RaceTypId == 4 && e.RacestatusId != 1006).ToList();

                globaltimestamp = SetTimes(nr1, globaltimestamp);
                globaltimestamp = SetTimes(nr2, globaltimestamp);
                globaltimestamp = SetTimes(nr3, globaltimestamp);
                globaltimestamp = SetTimes(nr4, globaltimestamp);
                globaltimestamp = SetTimes(nr5, globaltimestamp);
                globaltimestamp = SetTimes(nr6, globaltimestamp);
                globaltimestamp = SetTimes(nr7, globaltimestamp);
                globaltimestamp = SetTimes(nr8, globaltimestamp);
                globaltimestamp = SetTimes(nr9, globaltimestamp);
                globaltimestamp = SetTimes(nr10, globaltimestamp);
                globaltimestamp = SetTimes(nr11, globaltimestamp);
                globaltimestamp = SetTimes(nr12, globaltimestamp);
                globaltimestamp = SetTimes(nr13, globaltimestamp);
                globaltimestamp = SetTimes(nr14, globaltimestamp);
                globaltimestamp = SetTimes(nr15, globaltimestamp);
                globaltimestamp = SetTimes(nr16, globaltimestamp);
                globaltimestamp = SetTimes(nr17, globaltimestamp);
                globaltimestamp = SetTimes(nr18, globaltimestamp);
                globaltimestamp = SetTimes(nr19, globaltimestamp);
                globaltimestamp = SetTimes(nr20, globaltimestamp);
                globaltimestamp = SetTimes(nr21, globaltimestamp);
                globaltimestamp = SetTimes(nr22, globaltimestamp);
                globaltimestamp = SetTimes(nr23, globaltimestamp);
                globaltimestamp = SetTimes(nr24, globaltimestamp);
                globaltimestamp = SetTimes(nr25, globaltimestamp);
                globaltimestamp = SetTimes(nr26, globaltimestamp);
                globaltimestamp = SetTimes(nr27, globaltimestamp);
                globaltimestamp = SetTimes(nr28, globaltimestamp);
                globaltimestamp = SetTimes(nr29, globaltimestamp);
                globaltimestamp = SetTimes(nr30, globaltimestamp);
                globaltimestamp = SetTimes(nr31, globaltimestamp);
                globaltimestamp = SetTimes(nr32, globaltimestamp);
                globaltimestamp = SetTimes(nr33, globaltimestamp);
                globaltimestamp = SetTimes(nr34, globaltimestamp);
                globaltimestamp = SetTimes(nr35, globaltimestamp);
                globaltimestamp = SetTimes(nr36, globaltimestamp);
                globaltimestamp = SetTimes(nr37, globaltimestamp);
                globaltimestamp = SetTimes(nr38, globaltimestamp);
                globaltimestamp = SetTimes(nr39, globaltimestamp);
                globaltimestamp = SetTimes(nr40, globaltimestamp);
                globaltimestamp = SetTimes(nr41, globaltimestamp);
                globaltimestamp = SetTimes(nr42, globaltimestamp);
                globaltimestamp = SetTimes(nr43, globaltimestamp);
                globaltimestamp = SetTimes(nr44, globaltimestamp);                
                globaltimestamp = SetTimes(nr48, globaltimestamp);
                globaltimestamp = SetTimes(nr49, globaltimestamp);
                globaltimestamp = SetTimes(nr50, globaltimestamp);
                globaltimestamp = SetTimes(nr51, globaltimestamp);
                globaltimestamp = SetTimes(nr52, globaltimestamp);
                globaltimestamp = SetTimes(nr53, globaltimestamp);
                globaltimestamp = SetTimes(nr54, globaltimestamp);
                globaltimestamp = SetTimes(nr55, globaltimestamp);
                globaltimestamp = SetTimes(nr56, globaltimestamp);
                globaltimestamp = SetTimes(nr57, globaltimestamp);
                globaltimestamp = SetTimes(nr58, globaltimestamp);
                globaltimestamp = SetTimes(nr59, globaltimestamp);
                globaltimestamp = SetTimes(nr60, globaltimestamp);
                globaltimestamp = SetTimes(nr61, globaltimestamp);
                globaltimestamp = SetTimes(nr62, globaltimestamp);
                globaltimestamp = SetTimes(nr63, globaltimestamp);
                globaltimestamp = SetTimes(nr64, globaltimestamp);
                globaltimestamp = SetTimes(nr65, globaltimestamp);
                globaltimestamp = SetTimes(nr66, globaltimestamp);
                globaltimestamp = SetTimes(nr67, globaltimestamp);
                globaltimestamp = SetTimes(nr68, globaltimestamp);
                globaltimestamp = SetTimes(nr69, globaltimestamp);
                globaltimestamp = SetTimes(nr70, globaltimestamp);
                globaltimestamp = SetTimes(nr71, globaltimestamp);
                globaltimestamp = SetTimes(nr72, globaltimestamp);
                globaltimestamp = SetTimes(nr73, globaltimestamp);
                globaltimestamp = SetTimes(nr74, globaltimestamp);
                globaltimestamp = SetTimes(nr75, globaltimestamp);
                globaltimestamp = SetTimes(nr76, globaltimestamp);
                globaltimestamp = SetTimes(nr77, globaltimestamp);
                globaltimestamp = SetTimes(nr78, globaltimestamp);
                globaltimestamp = SetTimes(nr79, globaltimestamp);
                globaltimestamp = SetTimes(nr80, globaltimestamp);
                globaltimestamp = SetTimes(nr81, globaltimestamp);
                globaltimestamp = SetTimes(nr82, globaltimestamp);
                globaltimestamp = SetTimes(nr83, globaltimestamp);
                globaltimestamp = SetTimes(nr84, globaltimestamp);
                globaltimestamp = SetTimes(nr85, globaltimestamp);
                globaltimestamp = SetTimes(nr86, globaltimestamp);
                globaltimestamp = SetTimes(nr87, globaltimestamp);
                globaltimestamp = SetTimes(nr88, globaltimestamp);
                globaltimestamp = SetTimes(nr89, globaltimestamp);
                globaltimestamp = SetTimes(nr90, globaltimestamp);
                globaltimestamp = SetTimes(nr91, globaltimestamp);
                globaltimestamp = SetTimes(nr92, globaltimestamp);
                globaltimestamp = SetTimes(nr93, globaltimestamp);
                globaltimestamp = SetTimes(nr94, globaltimestamp);
                globaltimestamp = SetTimes(nr95, globaltimestamp);
                globaltimestamp = SetTimes(nr96, globaltimestamp);
                globaltimestamp = SetTimes(nr97, globaltimestamp);
                globaltimestamp = SetTimes(nr98, globaltimestamp);
                globaltimestamp = SetTimes(nr99, globaltimestamp);
                globaltimestamp = SetTimes(nr100, globaltimestamp);
                globaltimestamp = SetTimes(nr101, globaltimestamp);
                globaltimestamp = SetTimes(nr102, globaltimestamp);
                globaltimestamp = SetTimes(nr103, globaltimestamp);
                globaltimestamp = SetTimes(nr104, globaltimestamp);
                globaltimestamp = SetTimes(nr105, globaltimestamp);                
                globaltimestamp = SetTimes(nr109, globaltimestamp);
                globaltimestamp = SetTimes(nr110, globaltimestamp);
                globaltimestamp = SetTimes(nr111, globaltimestamp);
                globaltimestamp = SetTimes(nr112, globaltimestamp);
                globaltimestamp = SetTimes(nr113, globaltimestamp);
                globaltimestamp = SetTimes(nr114, globaltimestamp);
                globaltimestamp = SetTimes(nr115, globaltimestamp);
                globaltimestamp = SetTimes(nr116, globaltimestamp);
                globaltimestamp = SetTimes(nr117, globaltimestamp);
                globaltimestamp = SetTimes(nr118, globaltimestamp);
                globaltimestamp = SetTimes(nr119, globaltimestamp);                
            }
            else
            {
                DateTime globaltimestamp = regatta.FromDate;

                var races = _context.Races.Where(e => e.RacestatusId == 1).ToList();

                SetTimes(races, globaltimestamp);
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

                if (timestamp.Hour >= 18)
                {
                    timestamp = timestamp.AddHours(14);
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
