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
    public class OldclassController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OldclassController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Oldclass
        public async Task<IActionResult> Index()
        {
            return View(await _context.Oldclass.ToListAsync());
        }

        // GET: Oldclass/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oldclass = await _context.Oldclass
                .SingleOrDefaultAsync(m => m.OldclassId == id);
            if (oldclass == null)
            {
                return NotFound();
            }

            return View(oldclass);
        }

        // GET: Oldclass/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Oldclass/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OldclassId,Name,FromAge,ToAge")] Oldclass oldclass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(oldclass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(oldclass);
        }

        // GET: Oldclass/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oldclass = await _context.Oldclass.SingleOrDefaultAsync(m => m.OldclassId == id);
            if (oldclass == null)
            {
                return NotFound();
            }
            return View(oldclass);
        }

        // POST: Oldclass/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OldclassId,Name,FromAge,ToAge")] Oldclass oldclass)
        {
            if (id != oldclass.OldclassId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(oldclass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OldclassExists(oldclass.OldclassId))
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
            return View(oldclass);
        }

        // GET: Oldclass/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oldclass = await _context.Oldclass
                .SingleOrDefaultAsync(m => m.OldclassId == id);
            if (oldclass == null)
            {
                return NotFound();
            }

            return View(oldclass);
        }

        // POST: Oldclass/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var oldclass = await _context.Oldclass.SingleOrDefaultAsync(m => m.OldclassId == id);
            _context.Oldclass.Remove(oldclass);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OldclassExists(int id)
        {
            return _context.Oldclass.Any(e => e.OldclassId == id);
        }
    }
}
