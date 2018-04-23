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
    public class RaceTypController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RaceTypController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RaceTyp
        public async Task<IActionResult> Index()
        {
            return View(await _context.RaceTyps.ToListAsync());
        }

        // GET: RaceTyp/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raceTyp = await _context.RaceTyps
                .SingleOrDefaultAsync(m => m.RaceTypId == id);
            if (raceTyp == null)
            {
                return NotFound();
            }

            return View(raceTyp);
        }

        // GET: RaceTyp/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RaceTyp/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RaceTypId,Name,isFinal")] RaceTyp raceTyp)
        {
            if (ModelState.IsValid)
            {
                _context.Add(raceTyp);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(raceTyp);
        }

        // GET: RaceTyp/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raceTyp = await _context.RaceTyps.SingleOrDefaultAsync(m => m.RaceTypId == id);
            if (raceTyp == null)
            {
                return NotFound();
            }
            return View(raceTyp);
        }

        // POST: RaceTyp/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RaceTypId,Name,isFinal")] RaceTyp raceTyp)
        {
            if (id != raceTyp.RaceTypId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(raceTyp);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RaceTypExists(raceTyp.RaceTypId))
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
            return View(raceTyp);
        }

        // GET: RaceTyp/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raceTyp = await _context.RaceTyps
                .SingleOrDefaultAsync(m => m.RaceTypId == id);
            if (raceTyp == null)
            {
                return NotFound();
            }

            return View(raceTyp);
        }

        // POST: RaceTyp/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var raceTyp = await _context.RaceTyps.SingleOrDefaultAsync(m => m.RaceTypId == id);
            _context.RaceTyps.Remove(raceTyp);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RaceTypExists(int id)
        {
            return _context.RaceTyps.Any(e => e.RaceTypId == id);
        }
    }
}
