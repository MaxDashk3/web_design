using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BookShop.Models;

namespace BookShop.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public DbSet<Book> Books { get; set; }
		public DbSet<Purchase> Purchase { get; set; }

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}
	}
}