using BLL.Infrastructure.Models;

namespace BLL.Contracts;
public interface IUserService
{
	UserProfileModel GetUserProfileById(Guid userId);

	void UpdateUserProfile(UserProfileInfo model);

	UserModel LoginUser(string login, string password);

	void RegisterUser(RegisterUserModel model);

	UserModel GetUserByRefreshToken(Guid refreshToken);

	Guid CreateRefreshToken(Guid userId);
}
