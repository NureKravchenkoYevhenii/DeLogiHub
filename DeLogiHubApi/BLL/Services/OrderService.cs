using AutoMapper;
using BLL.Contracts;
using BLL.Infrastructure.Models.Order;
using DAL.Contracts;
using Domain.Models;
using Infrastructure.Exceptions;

namespace BLL.Services;
public class OrderService : IOrderService
{
	private readonly Lazy<IUnitOfWork> _unitOfWork;
	private readonly Lazy<IMapper> _mapper;
	private readonly Lazy<IRepository<Order>> _orders;

	public OrderService(
		Lazy<IUnitOfWork> unitOfWork,
		Lazy<IMapper> mapper)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;

		_orders = _unitOfWork.Value.GetLazyRepository<Order>();
	}

	public void Add(OrderModel orderModel)
	{
		var order = _mapper.Value.Map<Order>(orderModel);
		order.CreatedOn = DateTime.UtcNow;

		_orders.Value.Add(order);
	}

	public void Delete(Guid orderId)
	{
		var order = _orders.Value.GetById(orderId);
		if (order == null)
			return;

		_orders.Value.Remove(order);
	}

	public List<OrderModel> GetAll()
	{
		var orders = _orders.Value.GetAll().ToList();
		var ordersModel = _mapper.Value.Map<List<OrderModel>>(orders);

		return ordersModel;
	}

	public List<OrderModel> GetAllByCustomerId(Guid customerId)
	{
		var customerOrders = _orders.Value
			.GetList(o => o.CustomerId == customerId)
			.ToList();
		var customerOrdersModel = _mapper.Value.Map<List<OrderModel>>(customerOrders);

		return customerOrdersModel;
	}

	public OrderModel GetById(Guid orderId)
	{
		var order = _orders.Value.GetById(orderId)
			?? throw new EntityNotFoundException("Замовлення не знайдено");
		var orderModel = _mapper.Value.Map<OrderModel>(order);

		return orderModel;
	}

	public void Update(OrderModel orderModel)
	{
		var order = _orders.Value.GetById(orderModel.Id)
			?? throw new EntityNotFoundException("Замовлення не знайдено");
		_mapper.Value.Map(orderModel, order);
		_orders.Value.Update(order);
	}
}
