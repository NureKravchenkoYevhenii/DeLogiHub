using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.EntityConfigurations;
public class UserProfileConfiguration
	: IEntityTypeConfiguration<UserProfile>
{
	public void Configure(EntityTypeBuilder<UserProfile> builder)
	{
		builder.HasKey(up => up.Id);
	}
}
