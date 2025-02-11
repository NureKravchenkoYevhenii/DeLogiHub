namespace BLL.Infrastructure.Models;
public class UserProfileInfo
{
	public Guid Id { get; set; }

	public string FirstName { get; set; } = null!;

	public string LastName { get; set; } = null!;

	public byte[]? ProfilePicture { get; set; }

	public string Address { get; set; } = null!;

	public string PhoneNumber { get; set; } = null!;

	public DateTime BirthDate { get; set; }

	public string Email { get; set; } = null!;
}
