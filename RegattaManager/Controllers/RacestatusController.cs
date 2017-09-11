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
    public class RacestatusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RacestatusController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Racestatus
        public async Task<IActionResult> Index()
        {
            return View(await _context.Racestati.ToListAsync());
        }

        // GET: Racestatus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racestatus = await _context.Racestati
                .SingleOrDefaultAsync(m => m.RacestatusId == id);
            if (racestatus == null)
            {
                return NotFound();
            }

            return View(racestatus);
        }

        // GET: Racestatus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Racestatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RacestatusId,Name")] Racestatus racestatus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(racestatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(racestatus);
        }

        // GET: Racestatus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racestatus = await _context.Racestati.SingleOrDefaultAsync(m => m.RacestatusId == id);
            if (racestatus == null)
            {
                return NotFound();
            }
            return View(racestatus);
        }

        // POST: Racestatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RacestatusId,Name")] Racestatus racestatus)
        {
            if (id != racestatus.RacestatusId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(racestatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RacestatusExists(racestatus.RacestatusId))
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
            return View(racestatus);
        }

        // GET: Racestatus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racestatus = await _context.Racestati
                .SingleOrDefaultAsync(m => m.RacestatusId == id);
            if (racestatus == null)
            {
                return NotFound();
            }

            return View(racestatus);
        }

        // POST: Racestatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var racestatus = await _context.Racestati.SingleOrDefaultAsync(m => m.RacestatusId == id);
            _context.Racestati.Remove(racestatus);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RacestatusExists(int id)
        {
            return _context.Racestati.Any(e => e.RacestatusId == id);
        }
    }
}
