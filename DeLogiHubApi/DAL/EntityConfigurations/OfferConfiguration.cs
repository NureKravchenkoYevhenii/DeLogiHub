using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.EntityConfigurations;
public class OfferConfiguration
	: IEntityTypeConfiguration<Offer>
{
	public void Configure(EntityTypeBuilder<Offer> builder)
	{
		builder.HasKey(up => up.Id);
	}
}
