using Infrastructure.Enums;

namespace BLL.Infrastructure.Models;
public class UserModel
{
	public Guid Id { get; set; }

	public Role Role { get; set; }
}
