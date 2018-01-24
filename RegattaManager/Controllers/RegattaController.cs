﻿using System;
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
            ViewData["ClubId"] = new SelectList(_context.Clubs, "ClubId", "Name");
            ViewData["WaterId"] = new SelectList(_context.Waters, "WaterId", "Name");
            return View();
        }

        // POST: Regatta/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RegattaId,Name,FromDate,ToDate,ClubId,WaterId")] Regatta regatta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(regatta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClubId"] = new SelectList(_context.Clubs, "ClubId", "Name", regatta.ClubId);
            ViewData["WaterId"] = new SelectList(_context.Waters, "WaterId", "Name", regatta.WaterId);
            return View(regatta);
        }

        // GET: Regatta/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var regatta = await _context.Regattas.SingleOrDefaultAsync(m => m.RegattaId == id);
            if (regatta == null)
            {
                return NotFound();
            }
            ViewData["ClubId"] = new SelectList(_context.Clubs, "ClubId", "Name", regatta.ClubId);
            ViewData["WaterId"] = new SelectList(_context.Waters, "WaterId", "Name", regatta.WaterId);
            return View(regatta);
        }

        // POST: Regatta/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RegattaId,Name,FromDate,ToDate,ClubId,WaterId,Choosen")] Regatta regatta)
        {
            if (id != regatta.RegattaId)
            {
                return NotFound();
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
            return View(regatta);
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
        public IActionResult Oldclasses(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var regatta = _context.Regattas.Include(e => e.RegattaOldclasses).FirstOrDefault(r => r.RegattaId == id);
            var selectedRegattaOldclasses = _context.RegattaOldclasses.Where(m => m.RegattaId == id).Select(e => e.OldclassId).ToList();
            ViewData["OldclassId"] = new MultiSelectList(_context.Oldclasses,"OldclassId","Name",selectedRegattaOldclasses);            

            return View(regatta);
        }

        [HttpPost]
        public IActionResult Oldclasses(int RegattaId, IEnumerable<int> OldclassId)
        {
            foreach(int oid in OldclassId)
            {
                _context.Regattas.Include(e => e.RegattaOldclasses).FirstOrDefault(m => m.RegattaId == RegattaId).RegattaOldclasses.Add(new RegattaOldclass { RegattaId = RegattaId, OldclassId = oid });
            }            
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult CampingFees(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var regatta = _context.Regattas.Include(e => e.RegattaCampingFees).FirstOrDefault(r => r.RegattaId == id);
            var selectedCampingFees = _context.RegattaCampingFees.Where(m => m.RegattaId == id).Select(e => e.CampingFeeId).ToList();
            ViewData["CampingFeeId"] = new MultiSelectList(_context.CampingFees, "CampingFeeId", "Name", selectedCampingFees);

            return View(regatta);
        }

    /*    public IActionResult AddRaceclass(int id, int rcid)
        {
            _context.Regattas.Include(e => e.RegattaRaceclasses).FirstOrDefault(m => m.RegattaId == id).RegattaRaceclasses.Add(new RegattaRaceclass { RegattaId = id, RaceclassId = rcid });
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        */
        private bool RegattaExists(int id)
        {
            return _context.Regattas.Any(e => e.RegattaId == id);
        }
    }
}
