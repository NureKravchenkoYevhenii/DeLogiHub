using AutoMapper;
using BLL.Infrastructure.Models.Offers;
using Domain.Models;

namespace BLL.Infrastructure.Automapper;
public class OfferProfile : Profile
{
	public OfferProfile()
	{
		CreateMap<Offer, OfferModel>()
			.ReverseMap();
	}
}
