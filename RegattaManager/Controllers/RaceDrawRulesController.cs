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
    public class RaceDrawRulesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RaceDrawRulesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RaceDrawRules
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RaceDrawRules.Include(r => r.RaceDraw).Include(r => r.RaceTyp);            
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RaceDrawRules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raceDrawRules = await _context.RaceDrawRules
                .Include(r => r.RaceDraw)
                .Include(r => r.RaceTyp)
                .SingleOrDefaultAsync(m => m.RaceDrawRuleId == id);
            if (raceDrawRules == null)
            {
                return NotFound();
            }

            return View(raceDrawRules);
        }

        // GET: RaceDrawRules/Create
        public IActionResult Create()
        {
            ViewData["RaceDrawId"] = new SelectList(_context.RaceDraws, "RaceDrawId", "Name");
            ViewData["RaceTypId"] = new SelectList(_context.RaceTyps, "RaceTypId", "Name");            
            ViewData["ToRaceTypId"] = new SelectList(_context.RaceTyps, "RaceTypId", "Name");
            return View();
        }

        // POST: RaceDrawRules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RaceDrawRuleId,RaceDrawId,RaceTypId,RaceSequence,PlacementFrom,PlacementTo,ToRaceTypId,ToRaceSequence")] RaceDrawRules raceDrawRules)
        {
            if (ModelState.IsValid)
            {
                _context.Add(raceDrawRules);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RaceDrawId"] = new SelectList(_context.RaceDraws, "RaceDrawId", "Name", raceDrawRules.RaceDrawId);
            ViewData["RaceTypId"] = new SelectList(_context.RaceTyps, "RaceTypId", "Name", raceDrawRules.RaceTypId);
            return View(raceDrawRules);
        }

        // GET: RaceDrawRules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raceDrawRules = await _context.RaceDrawRules.SingleOrDefaultAsync(m => m.RaceDrawRuleId == id);
            if (raceDrawRules == null)
            {
                return NotFound();
            }
            ViewData["RaceDrawId"] = new SelectList(_context.RaceDraws, "RaceDrawId", "Name", raceDrawRules.RaceDrawId);
            ViewData["RaceTypId"] = new SelectList(_context.RaceTyps, "RaceTypId", "Name", raceDrawRules.RaceTypId);
            return View(raceDrawRules);
        }

        // POST: RaceDrawRules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RaceDrawRuleId,RaceDrawId,RaceTypId,RaceSequence,PlacementFrom,PlacementTo,ToRaceTypId,ToRaceSequence")] RaceDrawRules raceDrawRules)
        {
            if (id != raceDrawRules.RaceDrawRuleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(raceDrawRules);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RaceDrawRulesExists(raceDrawRules.RaceDrawRuleId))
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
            ViewData["RaceDrawId"] = new SelectList(_context.RaceDraws, "RaceDrawId", "Name", raceDrawRules.RaceDrawId);
            ViewData["RaceTypId"] = new SelectList(_context.RaceTyps, "RaceTypId", "Name", raceDrawRules.RaceTypId);
            return View(raceDrawRules);
        }

        // GET: RaceDrawRules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raceDrawRules = await _context.RaceDrawRules
                .Include(r => r.RaceDraw)
                .Include(r => r.RaceTyp)
                .SingleOrDefaultAsync(m => m.RaceDrawRuleId == id);
            if (raceDrawRules == null)
            {
                return NotFound();
            }

            return View(raceDrawRules);
        }

        // POST: RaceDrawRules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var raceDrawRules = await _context.RaceDrawRules.SingleOrDefaultAsync(m => m.RaceDrawRuleId == id);
            _context.RaceDrawRules.Remove(raceDrawRules);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RaceDrawRulesExists(int id)
        {
            return _context.RaceDrawRules.Any(e => e.RaceDrawRuleId == id);
        }
    }
}
