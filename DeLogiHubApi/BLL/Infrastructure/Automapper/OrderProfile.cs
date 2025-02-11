using AutoMapper;
using BLL.Infrastructure.Models.Order;
using Domain.Models;

namespace BLL.Infrastructure.Automapper;
public class OrderProfile : Profile
{
	public OrderProfile()
	{
		CreateMap<Order, OrderModel>()
			.ReverseMap();
	}
}
