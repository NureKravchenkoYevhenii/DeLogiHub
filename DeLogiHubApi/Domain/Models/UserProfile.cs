using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Models;
public class UserProfile : BaseEntity
{
	[Required]
	public string FirstName { get; set; }

	[Required]
	public string LastName { get; set; }

	public byte[] ProfilePicture { get; set; }

	[Required]
	public string Address { get; set; }

	public string PhoneNumber { get; set; }

	[Required]
	public DateTime BirthDate { get; set; }

	[Required]
	public string Email { get; set; }

	#region Relations

	[JsonIgnore]
	public User User { get; set; }

	#endregion
}
