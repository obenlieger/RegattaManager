using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RegattaManager.Data;
using RegattaManager.Models;

namespace RegattaManager.Controllers
{
    [Authorize]
    public class StartingFeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StartingFeeController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: StartingFee
        public ActionResult Index()
        {
            var model = _context.StartingFees.Include(e => e.Boatclasses).Include(e => e.Oldclasses).ToList();
            return View(model);
        }

        // GET: StartingFee/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: StartingFee/Create
        public ActionResult Create()
        {
            ViewData["BoatclassId"] = new SelectList(_context.Boatclasses, "BoatclassId", "Name");
            ViewData["OldclassId"] = new SelectList(_context.Oldclasses, "OldclassId", "Name");
            return View();
        }

        // POST: StartingFee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StartingFee startingFee)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    _context.Add(startingFee);
                    _context.SaveChanges();
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StartingFee/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StartingFee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StartingFee/Delete/5
        public ActionResult Delete(int id)
        {
            var model = _context.StartingFees.FirstOrDefault(e => e.StartingFeeId == id);
            return View(model);
        }

        // POST: StartingFee/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {
            try
            {
                var model = _context.StartingFees.FirstOrDefault(e => e.StartingFeeId == id);
                _context.StartingFees.Remove(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}