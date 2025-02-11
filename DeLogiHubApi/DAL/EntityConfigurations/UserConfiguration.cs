using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.EntityConfigurations;
public class UserConfiguration
	: IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.HasKey(x => x.Id);
		builder.HasOne(u => u.UserProfile)
			.WithOne(up => up.User)
			.HasForeignKey<UserProfile>(up => up.Id)
			.OnDelete(DeleteBehavior.Cascade);
		builder.HasMany(u => u.RefreshTokens)
			.WithOne(rt => rt.User)
			.HasForeignKey(rt => rt.UserId)
			.OnDelete(DeleteBehavior.Cascade);
		builder.HasMany(u => u.Transports)
			.WithOne(t => t.Carrier)
			.HasForeignKey(t => t.CarrierId)
			.OnDelete(DeleteBehavior.Cascade);
		builder.HasMany(u => u.Orders)
			.WithOne(o => o.Customer)
			.HasForeignKey(o => o.CustomerId)
			.OnDelete(DeleteBehavior.Cascade);
		builder.HasMany(u => u.Offers)
			.WithOne(o => o.Carrier)
			.HasForeignKey(o => o.CarrierId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
