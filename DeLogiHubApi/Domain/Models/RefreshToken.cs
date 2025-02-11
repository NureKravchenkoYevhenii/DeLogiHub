using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;
public class RefreshToken : BaseEntity
{
	[Required]
	public Guid Token { get; set; }

	[Required]
	public DateTime ExpiresOnUtc { get; set; }

	[Required]
	public Guid UserId { get; set; }

	#region Relations

	[JsonIgnore]
	public User User { get; set; } = null!;

	#endregion
}
