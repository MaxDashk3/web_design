﻿using Microsoft.AspNetCore.Mvc;
using Movies.Models;
using Movies.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Movies.ViewModels;

namespace Movies.Controllers
{
    public class SessionController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SessionController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var movies = _db.Movies
                .Include
                (m => m.Sessions
                .OrderBy(s => s.TimeDate))
                .ThenInclude(s => s.Hall)
                .Select(m => new MovieViewModel(m))
                .ToList();
            
            return View(movies);
        }
        public IActionResult Create()
        {
            ViewBag.Movies = _db.Movies.ToList();
            ViewBag.Halls = _db.Halls.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(SessionViewModel sessionViewModel)
        {
            _db.Sessions.Add(new Session(sessionViewModel));
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _db.Sessions == null)
            {
                return NotFound();
            }

            var session = await _db.Sessions.FindAsync(id);
            if (session == null)
            {
                return NotFound();
            }
            ViewBag.Movies = _db.Movies.ToList();
            ViewBag.Halls = _db.Halls.ToList();
            return View(new SessionViewModel(session));
        }

        // POST: Sessions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SessionViewModel vmodel)
        {
            var session = new Session(vmodel);
            if (id != session.Id)
            {
                return NotFound();
            }
            try
            {
                _db.Update(session);
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SessionExists(session.Id))
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

        // GET: Sessions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _db.Sessions == null)
            {
                return NotFound();
            }

            var session = await _db.Sessions
                .Include(s => s.Hall)
                .Include(s => s.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (session == null)
            {
                return NotFound();
            }

            var vmodel = new SessionViewModel(session); 
            return View(vmodel);
        }

        // POST: Sessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_db.Sessions == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Sessions'  is null.");
            }
            var session = await _db.Sessions.FindAsync(id);
            if (session != null)
            {
                _db.Sessions.Remove(session);
                var ticketsToRemove = _db.Tickets.Where(t => t.SessionId == session.Id);
                _db.Tickets.RemoveRange(ticketsToRemove);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int? id)
        {
            if (id == null || _db.Sessions == null)
            {
                return NotFound();
            }

            var session = _db.Sessions
                .Include(s => s.Hall)
                .Include(s => s.Movie)
                .FirstOrDefault(m => m.Id == id);
            if (session == null)
            {
                return NotFound();
            }

            return View(session);
        }

        private bool SessionExists(int id)
        {
            return (_db.Sessions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

