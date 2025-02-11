using Infrastructure.Enums;
using System.ComponentModel.DataAnnotations;

namespace BLL.Infrastructure.Models;
public class RegisterUserModel
{
	[Required]
	public Guid UserId { get; set; }

	[Required]
	public string Login { get; set; } = null!;

	[Required]
	public string Password { get; set; } = null!;

	[Required]
	public string FirstName { get; set; } = null!;

	[Required]
	public string LastName { get; set; } = null!;

	public byte[]? ProfilePicture { get; set; }

	[Required]
	public string Address { get; set; } = null!;

	[Required]
	public string PhoneNumber { get; set; } = null!;

	[Required]
	public DateTime BirthDate { get; set; }

	[Required]
	public string Email { get; set; } = null!;

	[Required]
	public Role Role { get; set; }
}
