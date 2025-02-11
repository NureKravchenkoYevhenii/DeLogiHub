using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.DbContexts;
public class DeLogiHubDbContext : DbContext
{
	public DbSet<User> Users { get; set; }

	public DbSet<UserProfile> UserProfiles { get; set; }

	public DbSet<RefreshToken> RefreshTokens { get; set; }

	public DbSet<Transport> Transports { get; set; }

	public DbSet<Order> Orders { get; set; }

	public DbSet<Offer> Offers { get; set; }

	public DeLogiHubDbContext(DbContextOptions<DeLogiHubDbContext> options)
		: base(options)
	{ }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(DeLogiHubDbContext).Assembly);

		base.OnModelCreating(modelBuilder);
	}
}
