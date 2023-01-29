using Kartel.Entities;
using Kartel.Environment;
using Microsoft.EntityFrameworkCore;

namespace Kartel.Locale.Google;

public class LocaleDbContext : DbContext
{
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlite(@"Data Source=Database.db");
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		foreach (var type in typeof(Person).Assembly.GetTypes())
			modelBuilder.Ignore(type);

		modelBuilder.Entity<Shop>().OwnsOne(building => building.Address, builder => { builder.Ignore(x => x.Lines); }).HasKey(b => b.Id);
		modelBuilder.Entity<Shop>().Property(building => building.Latitude);
		modelBuilder.Entity<Shop>().Property(building => building.Longitude);

		base.OnModelCreating(modelBuilder);
	}

	public DbSet<Shop> Shops { get; set; }
	
	public DbSet<ShopLocation> ShopLocations { get; set; }
}