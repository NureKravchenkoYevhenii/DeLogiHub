using BLL.Contracts;
using BLL.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DeLogiHubApi.Controllers;
[Area("transports")]
[Route("api/[area]")]
[ApiController]
[Authorize]
public class TransportsController : DeLogiHubBaseController
{
	private readonly ITransportService _transportService;

	public TransportsController(
		ITransportService transportService)
	{
		_transportService = transportService;
	}

	[HttpGet("get-all-by-carrier")]
	[ProducesResponseType(typeof(List<TransportModel>), (int)HttpStatusCode.OK)]
	public IActionResult GetTransportsByCarrier([FromQuery] Guid carrierId)
	{
		var carrierTransports = _transportService.GetAllByUserId(carrierId);
		return Ok(carrierTransports);
	}

	[HttpGet("get")]
	[ProducesResponseType(typeof(TransportModel), (int)HttpStatusCode.OK)]
	public IActionResult GetTransport([FromQuery] Guid id)
	{
		var transport = _transportService.GetById(id);
		return Ok(transport);
	}

	[HttpPost("add")]
	public IActionResult AddTransport([FromBody] TransportModel transportModel)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		_transportService.Add(transportModel);
		return Ok();
	}

	[HttpPost("update")]
	public IActionResult UpdateTransport([FromBody] TransportModel transportModel)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		_transportService.Update(transportModel);
		return Ok();
	}

	[HttpDelete("delete")]
	public IActionResult DeleteTransport([FromQuery] Guid id)
	{
		_transportService.Delete(id);
		return Ok();
	}
}
