using AutoMapper;
using BLL.Contracts;
using BLL.Infrastructure.Models;
using DAL.Contracts;
using Domain.Models;
using Infrastructure.Configs;
using Infrastructure.Enums;
using Infrastructure.Exceptions;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;
public class UserService : IUserService
{
	private readonly Lazy<AuthOptions> _authOptions;
	private readonly Lazy<IUnitOfWork> _unitOfWork;
	private readonly Lazy<IMapper> _mapper;
	private readonly Lazy<IRepository<User>> _users;
	private readonly Lazy<IRepository<UserProfile>> _userProfiles;

	public UserService(
		Lazy<AuthOptions> authOptions,
		Lazy<IUnitOfWork> unitOfWork,
		Lazy<IMapper> mapper)
	{
		_authOptions = authOptions;
		_unitOfWork = unitOfWork;
		_mapper = mapper;

		_users = _unitOfWork.Value.GetLazyRepository<User>();
		_userProfiles = _unitOfWork.Value.GetLazyRepository<UserProfile>();
	}

	public Guid CreateRefreshToken(Guid userId)
	{
		var user = _users.Value.GetAll()
			.Include(u => u.RefreshTokens)
			.FirstOrDefault(u => u.Id == userId)
				?? throw new EntityNotFoundException("Користувача не знайдено");

		var refreshToken = new RefreshToken()
		{
			Token = Guid.NewGuid(),
			UserId = userId,
			ExpiresOnUtc = DateTime.UtcNow
				.AddSeconds(_authOptions.Value.RefreshTokenLifetime),
		};

		user.RefreshTokens.Clear();
		user.RefreshTokens.Add(refreshToken);

		_unitOfWork.Value.Commit();

		return refreshToken.Token;
	}

	public UserModel GetUserByRefreshToken(Guid refreshToken)
	{
		var user = _users.Value.GetAll()
			.Include(u => u.RefreshTokens)
			.FirstOrDefault(u => u.RefreshTokens.Any(rt =>
				rt.Token == refreshToken
				&& rt.ExpiresOnUtc >= DateTime.UtcNow)
			) ?? throw new DeLogiHubException("Недійсний токен оновлення");

		var userModel = _mapper.Value.Map<UserModel>(user);

		return userModel;
	}

	public UserProfileModel GetUserProfileById(Guid userId)
	{
		var user = _users.Value.GetAll()
			.Include(u => u.UserProfile)
			.FirstOrDefault(u => u.Id == userId)
				?? throw new EntityNotFoundException("Користувача не знайдено");

		var userProfile = _mapper.Value.Map<UserProfileModel>(user);

		return userProfile;
	}

	public UserModel LoginUser(string login, string password)
	{
		var user = _users.Value.Get(u => u.Login == login
			|| u.UserProfile.Email == login);

		if (user is null
			|| !HashHelper.VerifyPassword(password, user.PasswordSalt, user.PasswordHash))
		{
			throw new UnauthorizedAccessException("Недійсний логін або пароль");
		}

		var userModel = _mapper.Value.Map<UserModel>(user);

		return userModel;
	}

	public void RegisterUser(RegisterUserModel model)
	{
		if (_users.Value.GetAll().Any(x => x.Login == model.Login))
			throw new DeLogiHubException("Користувач з таким логіном існує");

		if (_users.Value.GetAll().Any(x => x.UserProfile.Email == model.Email))
			throw new DeLogiHubException("Користувач з таким email існує");

		var (salt, passwordHash) = HashHelper.GenerateNewPasswordHash(model.Password);
		var user = _mapper.Value.Map<User>(model);
		var userProfile = _mapper.Value.Map<UserProfile>(model);
		userProfile.ProfilePicture ??= [];

		user.PasswordSalt = salt;
		user.PasswordHash = passwordHash;
		user.RegistrationDate = DateTime.UtcNow;
		user.UserProfile = userProfile;

		_users.Value.Add(user);
	}

	public void UpdateUserProfile(UserProfileInfo model)
	{
		var profile = _userProfiles.Value.Get(p => p.Id == model.Id)
			?? throw new EntityNotFoundException("Профіль користувача не знайдено");

		_mapper.Value.Map(model, profile);
		_unitOfWork.Value.Commit();
	}
}
