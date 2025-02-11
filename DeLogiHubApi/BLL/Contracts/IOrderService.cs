using BLL.Infrastructure.Models.Order;

namespace BLL.Contracts;
public interface IOrderService
{
	List<OrderModel> GetAll();

	List<OrderModel> GetAllByCustomerId(Guid customerId);

	OrderModel GetById(Guid orderId);

	void Add(OrderModel orderModel);

	void Update(OrderModel orderModel);

	void Delete(Guid orderId);
}
