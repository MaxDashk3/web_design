﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movies.Data;
using Movies.Models;
using Movies.ViewModels;

namespace Movies.Controllers
{
    public class PurchasesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PurchasesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Purchases
        public IActionResult Index()
        {
            var purchases = _context.Purchases
                .Include(p => p.Tickets)
                .Include("Tickets.Session")
                .Include("Tickets.Session.Movie")
                .Include("Tickets.Session.Hall")
                .Select(p => new PurchaseViewModel(p))
                .ToList();

            return View(purchases);
        }

        // GET: Purchases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Purchases == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchases
                .Include(p => p.Tickets)
                .Include("Tickets.Session.Movie")
                .Include("Tickets.Session.Hall")
                .FirstOrDefaultAsync(m => m.PurchaseId == id);
            if (purchase == null)
            {
                return NotFound();
            }

            return View(new PurchaseViewModel(purchase));
        }

        // GET: Purchases/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult BuyResult(PurchaseViewModel model)
        {
            var purchase = new Purchase(model);
            List<Ticket> tickets = _context.Tickets
            .Where(t => t.PurchaseId == null)
            .ToList();

            ViewBag.Name = purchase.Person;
            purchase.Date = DateTime.Now;
            _context.Purchases.Add(purchase);
            _context.SaveChanges();

            foreach(var t in tickets)
            {
                t.PurchaseId = purchase.PurchaseId;
                _context.Update(t);
                _context.SaveChanges();
            }
            return View();
        }

        // POST: Purchases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(PurchaseViewModel model)
        //{
        //    var purchase = new Purchase(model);
        //    _context.Add(purchase);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        // GET: Purchases/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Purchases == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchases.FindAsync(id);
            if (purchase == null)
            {
                return NotFound();
            }
            return View(new PurchaseViewModel(purchase));
        }

        // POST: Purchases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PurchaseViewModel model)
        {
            var purchase = new Purchase(model);
            if (id != purchase.PurchaseId)
            {
                return NotFound();
            }

            try
            {
                _context.Update(purchase);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseExists(purchase.PurchaseId))
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

        // GET: Purchases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Purchases == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchases
                .FirstOrDefaultAsync(m => m.PurchaseId == id);
            if (purchase == null)
            {
                return NotFound();
            }

            return View(new PurchaseViewModel(purchase));
        }

        // POST: Purchases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Purchases == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Purchases'  is null.");
            }
            var purchase = await _context.Purchases.FindAsync(id);
            if (purchase != null)
            {
                var ticketsToRemove = _context.Tickets
                    .Where(t => t.PurchaseId == purchase.PurchaseId).ToList();
                _context.Tickets.RemoveRange(ticketsToRemove);
                _context.Purchases.Remove(purchase);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchaseExists(int id)
        {
          return (_context.Purchases?.Any(e => e.PurchaseId == id)).GetValueOrDefault();
        }
    }
}
