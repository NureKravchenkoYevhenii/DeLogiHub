using Infrastructure.Enums;

namespace BLL.Infrastructure.Models;
public class UserProfileModel
{
	public Guid Id { get; set; }

	public string FirstName { get; set; } = null!;

	public string LastName { get; set; } = null!;

	public byte[]? ProfilePicture { get; set; }

	public string Address { get; set; } = null!;

	public string PhoneNumber { get; set; } = null!;

	public DateTime BirthDate { get; set; }

	public string BirthDateString { get; set; } = null!;

	public string Email { get; set; } = null!;

	public string Login { get; set; } = null!;

	public DateTime RegistrationDate { get; set; }

	public string RegistrationDateStr { get; set; } = null!;

	public Role Role { get; set; }
}
