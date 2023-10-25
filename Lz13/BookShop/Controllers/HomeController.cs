using BookShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BookShop.Models;
using BookShop.Data;

namespace BookShop.Controllers
{
	public class HomeController : Controller
	{
		private readonly ApplicationDbContext _dbContext;

		public HomeController(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public IActionResult Index()
		{
			IEnumerable<Book> books = _dbContext.Books;
			ViewBag.Books = books;
			return View();
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(Book book)
		{
			if(book != null)
			{
				_dbContext.Books.Add(book);
				_dbContext.SaveChanges();
			}
			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public ActionResult Buy(int? id) 
		{
			ViewBag.BookId = id ?? 0;
			return View();
		}

		[HttpPost]
		public string Buy(Purchase purchase)
		{
			purchase.Date = DateTime.Now;
			_dbContext.Purchase.Add(purchase);
			_dbContext.SaveChanges();
			return "Дякуємо " + purchase.Person + " за купівлю!";
		}
		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}