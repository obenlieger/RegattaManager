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
    public class StartboatstatusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StartboatstatusController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Startboatstatus
        public async Task<IActionResult> Index()
        {
            return View(await _context.Startboatstati.ToListAsync());
        }

        // GET: Startboatstatus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var startboatstatus = await _context.Startboatstati
                .SingleOrDefaultAsync(m => m.StartboatstatusId == id);
            if (startboatstatus == null)
            {
                return NotFound();
            }

            return View(startboatstatus);
        }

        // GET: Startboatstatus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Startboatstatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StartboatstatusId,Name")] Startboatstatus startboatstatus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(startboatstatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(startboatstatus);
        }

        // GET: Startboatstatus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var startboatstatus = await _context.Startboatstati.SingleOrDefaultAsync(m => m.StartboatstatusId == id);
            if (startboatstatus == null)
            {
                return NotFound();
            }
            return View(startboatstatus);
        }

        // POST: Startboatstatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StartboatstatusId,Name")] Startboatstatus startboatstatus)
        {
            if (id != startboatstatus.StartboatstatusId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(startboatstatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StartboatstatusExists(startboatstatus.StartboatstatusId))
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
            return View(startboatstatus);
        }

        // GET: Startboatstatus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var startboatstatus = await _context.Startboatstati
                .SingleOrDefaultAsync(m => m.StartboatstatusId == id);
            if (startboatstatus == null)
            {
                return NotFound();
            }

            return View(startboatstatus);
        }

        // POST: Startboatstatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var startboatstatus = await _context.Startboatstati.SingleOrDefaultAsync(m => m.StartboatstatusId == id);
            _context.Startboatstati.Remove(startboatstatus);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StartboatstatusExists(int id)
        {
            return _context.Startboatstati.Any(e => e.StartboatstatusId == id);
        }
    }
}
