using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RegattaManager.Data;
using RegattaManager.Models;

namespace RegattaManager.Controllers
{
    public class RaceDrawController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RaceDrawController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RaceDraw
        public async Task<IActionResult> Index()
        {
            return View(await _context.RaceDraws.ToListAsync());
        }

        // GET: RaceDraw/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raceDraw = await _context.RaceDraws
                .SingleOrDefaultAsync(m => m.RaceDrawId == id);
            if (raceDraw == null)
            {
                return NotFound();
            }

            var raceDrawRules = await _context.RaceDrawRules.Include(e => e.RaceDraw).Where(e => e.RaceDrawId == id).ToListAsync();
            var raceTyps = await _context.RaceTyps.ToListAsync();

            ViewBag.raceDrawRules = raceDrawRules;
            ViewBag.raceTyps = raceTyps;

            ViewData["RaceDrawId"] = new SelectList(_context.RaceDraws, "RaceDrawId", "Name");
            ViewData["RaceTypId"] = new SelectList(_context.RaceTyps, "RaceTypId", "Name");
            ViewData["ToRaceTypId"] = new SelectList(_context.RaceTyps, "RaceTypId", "Name");

            return View(raceDraw);
        }

        // GET: RaceDraw/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RaceDraw/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RaceDrawId,ReportedSBCountFrom,ReportedSBCountTo,VorlaufCount,HoffnungslaufCount,ZwischenlaufCount,EndlaufCount,isAbteilungslauf")] RaceDraw raceDraw)
        {
            if (ModelState.IsValid)
            {
                _context.Add(raceDraw);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(raceDraw);
        }

        // GET: RaceDraw/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raceDraw = await _context.RaceDraws.SingleOrDefaultAsync(m => m.RaceDrawId == id);
            if (raceDraw == null)
            {
                return NotFound();
            }
            return View(raceDraw);
        }

        // POST: RaceDraw/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RaceDrawId,ReportedSBCountFrom,ReportedSBCountTo,VorlaufCount,HoffnungslaufCount,ZwischenlaufCount,EndlaufCount,isAbteilungslauf")] RaceDraw raceDraw)
        {
            if (id != raceDraw.RaceDrawId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(raceDraw);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RaceDrawExists(raceDraw.RaceDrawId))
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
            return View(raceDraw);
        }

        // GET: RaceDraw/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raceDraw = await _context.RaceDraws
                .SingleOrDefaultAsync(m => m.RaceDrawId == id);
            if (raceDraw == null)
            {
                return NotFound();
            }

            return View(raceDraw);
        }

        // POST: RaceDraw/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var raceDraw = await _context.RaceDraws.SingleOrDefaultAsync(m => m.RaceDrawId == id);
            _context.RaceDraws.Remove(raceDraw);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddRaceDrawRule(int RaceDrawId, int RaceTypId, int RaceSequence, int PlacementFrom, int PlacementTo, int ToRaceTypId, int ToRaceSequence)
        {
            _context.RaceDrawRules.Add(new RaceDrawRules { RaceDrawId = RaceDrawId, RaceTypId = RaceTypId, RaceSequence = RaceSequence, PlacementFrom = PlacementFrom, PlacementTo = PlacementTo, ToRaceTypId = ToRaceTypId, ToRaceSequence = ToRaceSequence });
            _context.SaveChanges();

            return RedirectToAction("Details", new { id = RaceDrawId });
        }

        private bool RaceDrawExists(int id)
        {
            return _context.RaceDraws.Any(e => e.RaceDrawId == id);
        }
    }
}
