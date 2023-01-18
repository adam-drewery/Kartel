using Kartel.Entities;
using Kartel.Environment;
using Microsoft.EntityFrameworkCore;

namespace Kartel.PropertyMarket;

public class ZooplaDbContext : DbContext
{
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlite(@"Data Source=Database.db");
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		foreach (var type in typeof(Person).Assembly.GetTypes())
			modelBuilder.Ignore(type);

		modelBuilder.Entity<Building>().OwnsOne(building => building.Address, builder => { builder.Ignore(x => x.Lines); }).HasKey(b => b.Id);
		modelBuilder.Entity<Building>().Property(building => building.Latitude);
		modelBuilder.Entity<Building>().Property(building => building.Longitude);

		base.OnModelCreating(modelBuilder);
	}

	public DbSet<Building> Buildings { get; set; }
}