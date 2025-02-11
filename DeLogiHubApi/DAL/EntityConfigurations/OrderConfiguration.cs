using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.EntityConfigurations;
public class OrderConfiguration
	: IEntityTypeConfiguration<Order>
{
	public void Configure(EntityTypeBuilder<Order> builder)
	{
		builder.HasKey(up => up.Id);
		builder.HasMany(o => o.Offers)
			.WithOne(of => of.Order)
			.HasForeignKey(of => of.OrderId)
			.OnDelete(DeleteBehavior.NoAction);
	}
}
