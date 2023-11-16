using Microsoft.AspNetCore.Mvc;
using Movies.Models;
using Movies.Data;
using Microsoft.EntityFrameworkCore;

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
            var sessions = _db.Sessions
                .Include(s => s.Movie)
                .Include(s => s.Hall)
                .ToList();
            
            return View(sessions);
        }
        public IActionResult Create()
        {
            ViewBag.Movies = _db.Movies.ToList();
            ViewBag.Halls = _db.Halls.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Session session)
        {
            _db.Sessions.Add(session);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult MovieDetails(int id)
        {
            var movie = _db.Movies
                .Include(m => m.Genre)
                .Include(m => m.Sessions)
                .ThenInclude(s => s.Hall)
                .FirstOrDefault(x => x.Id == id);

            return View(movie);
        }
        public IActionResult CreateTicket(int id)
        {
            ViewBag.Session = _db.Sessions
                .Include(s => s.Movie)
                .Include(s => s.Hall)
                .FirstOrDefault(s => s.Id == id);

            return View();
        }
        [HttpPost]
        public IActionResult CreateTicket(Ticket ticket)
        {
            _db.Tickets.Add(ticket);
            _db.SaveChanges();

            return RedirectToAction("ShowTickets");
        }

        public IActionResult ShowTickets()
        {
            var tickets = _db.Tickets
                .Include(t => t.Session)
                .Include(t => t.Session.Movie)
                .Include(t => t.Session.Hall)
                .Include(t => t.Session.Movie.Genre)
                .ToList();

            return View(tickets);
        }

        [HttpGet]
        public IActionResult Buy(int? id)
        {
            ViewBag.Id = id ?? 0;
            return View();
        }
        [HttpPost]
        public IActionResult BuyResult(Purchase purchase)
        {
            ViewBag.Name = purchase.Person;
            purchase.Date = DateTime.Now;
            _db.Purchases.Add(purchase);
            _db.SaveChanges();
            return View();
        }
    }
}
