using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.EntityConfigurations;
public class TransportConfiguration
	: IEntityTypeConfiguration<Transport>
{
	public void Configure(EntityTypeBuilder<Transport> builder)
	{
		builder.HasKey(up => up.Id);
	}
}
