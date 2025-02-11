using AutoMapper;
using BLL.Infrastructure.Models;
using Domain.Models;

namespace BLL.Infrastructure.Automapper;
public class TransportProfile : Profile
{
	public TransportProfile()
	{
		CreateMap<Transport, TransportModel>()
			.ReverseMap();
	}
}
