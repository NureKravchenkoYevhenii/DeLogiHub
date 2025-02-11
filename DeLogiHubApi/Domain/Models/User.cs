using Infrastructure.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;
public class User : BaseEntity
{
	[Required]
	public string Login { get; set; }

	[Required]
	public string PasswordHash { get; set; }

	[Required]
	public string PasswordSalt { get; set; }

	[Required]
	public DateTime RegistrationDate { get; set; }

	[Required]
	public Role Role { get; set; }

	#region Relations

	[JsonIgnore]
	public ICollection<RefreshToken> RefreshTokens { get; set; }

	[JsonIgnore]
	public ICollection<Transport> Transports { get; set; }

	[JsonIgnore]
	public ICollection<Order> Orders { get; set; }

	[JsonIgnore]
	public ICollection<Offer> Offers { get; set; }

	[JsonIgnore]
	public UserProfile UserProfile { get; set; }

	#endregion
}
