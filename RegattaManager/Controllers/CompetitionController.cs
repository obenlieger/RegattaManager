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
    public class CompetitionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompetitionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Competition
        public ActionResult Index()
        {
            var model = _context.Competitions.Include(e => e.Raceclasses).Include(e => e.Boatclasses).ToList().OrderBy(e => e.Boatclasses.Name);
            return View(model);
        }

        // GET: Competition/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Competition/Create
        public ActionResult Create()
        {
            ViewData["BoatclassId"] = new SelectList(_context.Boatclasses, "BoatclassId", "Name");
            ViewData["RaceclassId"] = new SelectList(_context.Raceclasses, "RaceclassId", "Name");
            return View();
        }

        // POST: Competition/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Competition competition)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    _context.Add(competition);
                    _context.SaveChanges();
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]        
        public ActionResult CreateAll()
        {
            var bclist = _context.Boatclasses.ToList();
            var rclist = _context.Raceclasses.ToList();

            foreach(var bc in bclist)
            {
                foreach(var rc in rclist)
                {
                    _context.Competitions.Add(new Competition { BoatclassId = bc.BoatclassId, RaceclassId = rc.RaceclassId });
                }
            }
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // GET: Competition/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Competition/Edit/5
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

        // GET: Competition/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Competition/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}