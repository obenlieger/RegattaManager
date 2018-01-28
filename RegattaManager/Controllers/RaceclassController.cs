using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RegattaManager.Data;
using RegattaManager.Models;

namespace RegattaManager.Controllers
{
    [Authorize]
    public class RaceclassController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RaceclassController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Raceclass
        public async Task<IActionResult> Index()
        {
            return View(await _context.Raceclasses.ToListAsync());
        }

        // GET: Raceclass/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raceclass = await _context.Raceclasses
                .SingleOrDefaultAsync(m => m.RaceclassId == id);
            if (raceclass == null)
            {
                return NotFound();
            }

            return View(raceclass);
        }

        // GET: Raceclass/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Raceclass/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RaceclassId,Name,Length")] Raceclass raceclass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(raceclass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(raceclass);
        }

        // GET: Raceclass/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raceclass = await _context.Raceclasses.SingleOrDefaultAsync(m => m.RaceclassId == id);
            if (raceclass == null)
            {
                return NotFound();
            }
            return View(raceclass);
        }

        // POST: Raceclass/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RaceclassId,Name,Length")] Raceclass raceclass)
        {
            if (id != raceclass.RaceclassId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(raceclass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RaceclassExists(raceclass.RaceclassId))
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
            return View(raceclass);
        }

        // GET: Raceclass/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raceclass = await _context.Raceclasses
                .SingleOrDefaultAsync(m => m.RaceclassId == id);
            if (raceclass == null)
            {
                return NotFound();
            }

            return View(raceclass);
        }

        // POST: Raceclass/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var raceclass = await _context.Raceclasses.SingleOrDefaultAsync(m => m.RaceclassId == id);
            _context.Raceclasses.Remove(raceclass);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RaceclassExists(int id)
        {
            return _context.Raceclasses.Any(e => e.RaceclassId == id);
        }
    }
}
