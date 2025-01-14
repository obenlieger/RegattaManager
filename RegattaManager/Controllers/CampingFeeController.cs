﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RegattaManager.Data;
using RegattaManager.Models;

namespace RegattaManager.Controllers
{
    [Authorize]
    public class CampingFeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CampingFeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CampingFee
        public ActionResult Index()
        {
            var model = _context.CampingFees.ToList();
            return View(model);
        }

        // GET: CampingFee/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CampingFee/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CampingFee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CampingFee campingFee)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    _context.Add(campingFee);
                    _context.SaveChanges();
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CampingFee/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CampingFee/Edit/5
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

        // GET: CampingFee/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CampingFee/Delete/5
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