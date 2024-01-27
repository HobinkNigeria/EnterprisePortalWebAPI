using EnterprisePortalWebAPI.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace EnterprisePortalWebAPI.Core
{
	public class DatabaseContext(DbContextOptions options) : DbContext(options)
	{
		/// <summary>
		/// Menu
		/// </summary>
		public DbSet<Menu> Menus => Set<Menu>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Menu>()
					.Property(m => m.Price)
					.HasColumnType("decimal(18, 2)");
		}
	}
}
