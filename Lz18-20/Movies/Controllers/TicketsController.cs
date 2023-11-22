using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movies.Data;
using Movies.Models;

namespace Movies.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tickets
        public IActionResult Create(int id)
        {
            var session = _context.Sessions
                .Include(s => s.Movie)
                .Include(s => s.Hall)
                .Include(s => s.Tickets)
                .FirstOrDefault(s => s.Id == id);

            if (session != null)
            {
                var tickets = session.Tickets.ToList();
                var takenseats = new List<string>();

                foreach (var t in tickets)
                {
                    takenseats.Add($"{t.SeatRow} {t.SeatNum}");
                }

                ViewBag.Tickets = tickets;
                ViewBag.Session = session;
                ViewBag.TakenSeats = takenseats;
                return View();
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Create(int SessionId, string Seat)
        {
            var ticket = new Ticket();
            ticket.SessionId = SessionId;
            ticket.SeatRow = Convert.ToInt16(Seat.Remove(Seat.IndexOf(" ")));
            ticket.SeatNum = Convert.ToInt16(Seat.Remove(0, Seat.IndexOf(" ")));

            _context.Tickets.Add(ticket);
            _context.SaveChanges();

            return RedirectToAction("Index","Session");
        }

        public IActionResult Index()
        {
            var tickets = _context.Tickets
                .Include(t => t.Session)
                .Include(t => t.Session.Movie)
                .Include(t => t.Session.Hall)
                .Include(t => t.Session.Movie.Genre)
                .Include(t => t.Purchase)
                .ToList();

            return View(tickets);
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Session)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["SessionId"] = new SelectList(_context.Sessions, "Id", "Id", ticket.SessionId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SessionId,SeatRow,SeatNum")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
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
            ViewData["SessionId"] = new SelectList(_context.Sessions, "Id", "Id", ticket.SessionId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Session)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tickets == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Tickets'  is null.");
            }
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Cart()
        {
            List<Ticket> tickets = _context.Tickets
                .Include(t => t.Session)
                .Include(t => t.Session.Movie)
                .Include(t => t.Session.Hall)
                .Where(t => t.PurchaseId == null)
                .ToList();
            return View(tickets);
        }

        private bool TicketExists(int id)
        {
          return (_context.Tickets?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
