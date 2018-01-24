using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RegattaManager.Data;
using RegattaManager.Models;

namespace RegattaManager.Controllers
{
    public class WaterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WaterController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: Water
        public ActionResult Index()
        {
            var model = _context.Waters.ToList();
            return View(model);
        }

        // GET: Water/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Water/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Water/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Water water)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    _context.Add(water);
                    _context.SaveChanges();
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Water/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Water/Edit/5
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

        // GET: Water/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Water/Delete/5
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