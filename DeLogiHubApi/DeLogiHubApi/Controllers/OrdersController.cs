using BLL.Contracts;
using BLL.Infrastructure.Models.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DeLogiHubApi.Controllers;
[Area("orders")]
[Route("api/[area]")]
[ApiController]
[Authorize]
public class OrdersController : DeLogiHubBaseController
{
	private readonly IOrderService _orderService;

	public OrdersController(
		IOrderService orderService)
	{
		_orderService = orderService;
	}

	[HttpGet("get-all-by-customer")]
	[ProducesResponseType(typeof(List<OrderModel>), (int)HttpStatusCode.OK)]
	public IActionResult GetCustomerOrders([FromQuery] Guid customerId)
	{
		var customerOrders = _orderService.GetAllByCustomerId(customerId);
		return Ok(customerOrders);
	}

	[HttpGet("get-all")]
	[ProducesResponseType(typeof(List<OrderModel>), (int)HttpStatusCode.OK)]
	public IActionResult GetOrders()
	{
		var orders = _orderService.GetAll();
		return Ok(orders);
	}

	[HttpGet("get")]
	[ProducesResponseType(typeof(OrderModel), (int)HttpStatusCode.OK)]
	public IActionResult GetOrder([FromQuery] Guid id)
	{
		var order = _orderService.GetById(id);
		return Ok(order);
	}

	[HttpPost("add")]
	public IActionResult AddOrder([FromBody] OrderModel orderModel)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		_orderService.Add(orderModel);
		return Ok();
	}

	[HttpPost("update")]
	public IActionResult UpdateOrder([FromBody] OrderModel orderModel)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		_orderService.Update(orderModel);
		return Ok();
	}

	[HttpDelete("delete")]
	public IActionResult DeleteOrder([FromQuery] Guid id)
	{
		_orderService.Delete(id);
		return Ok();
	}
}
