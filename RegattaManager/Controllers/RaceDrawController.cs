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
        public async Task<IActionResult> Create([Bind("RaceDrawId,ReportedSBCountFrom,ReportedSBCountTo,VorlaufCount,HoffnungslaufCount,ZwischenlaufCount,EndlaufCount")] RaceDraw raceDraw)
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
        public async Task<IActionResult> Edit(int id, [Bind("RaceDrawId,ReportedSBCountFrom,ReportedSBCountTo,VorlaufCount,HoffnungslaufCount,ZwischenlaufCount,EndlaufCount")] RaceDraw raceDraw)
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

        private bool RaceDrawExists(int id)
        {
            return _context.RaceDraws.Any(e => e.RaceDrawId == id);
        }
    }
}
