using AutoMapper;
using BLL.Infrastructure.Models;
using Domain.Models;

namespace BLL.Infrastructure.Automapper;
public class UserProfile : Profile
{
	public UserProfile()
	{
		CreateMap<User, UserProfileModel>()
			.ForMember(dst => dst.FirstName,
				opt => opt.MapFrom(src => src.UserProfile.FirstName))
			.ForMember(dst => dst.LastName,
				opt => opt.MapFrom(src => src.UserProfile.LastName))
			.ForMember(dst => dst.ProfilePicture,
				opt => opt.MapFrom(src => src.UserProfile.ProfilePicture))
			.ForMember(dst => dst.PhoneNumber,
				opt => opt.MapFrom(src => src.UserProfile.PhoneNumber))
			.ForMember(dst => dst.Address,
				opt => opt.MapFrom(src => src.UserProfile.Address))
			.ForMember(dst => dst.BirthDate,
				opt => opt.MapFrom(src => src.UserProfile.BirthDate))
			.ForMember(dst => dst.BirthDateString,
				opt => opt.MapFrom(src => src.UserProfile.BirthDate.ToLocalTime().ToLongTimeString()))
			.ForMember(dst => dst.Email,
				opt => opt.MapFrom(src => src.UserProfile.Email))
			.ForMember(dst => dst.RegistrationDate,
				opt => opt.MapFrom(src => src.RegistrationDate.ToLocalTime()))
			.ForMember(dst => dst.RegistrationDateStr,
				opt => opt.MapFrom(src => src.RegistrationDate.ToLocalTime().ToLongTimeString()))
			.ReverseMap();

		CreateMap<RegisterUserModel, User>();

		CreateMap<RegisterUserModel, Domain.Models.UserProfile>();

		CreateMap<UserProfileInfo, Domain.Models.UserProfile>();

		CreateMap<User, UserModel>()
			.ReverseMap();
	}
}
