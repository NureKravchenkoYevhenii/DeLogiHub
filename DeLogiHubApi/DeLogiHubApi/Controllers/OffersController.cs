using BLL.Contracts;
using BLL.Infrastructure.Models.Offers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DeLogiHubApi.Controllers;
[Area("offers")]
[Route("api/[area]")]
[ApiController]
[Authorize]
public class OffersController : DeLogiHubBaseController
{
	private readonly IOfferService _offerService;

	public OffersController(
		IOfferService offerService)
	{
		_offerService = offerService;
	}

	[HttpPost("add")]
	public IActionResult AddOffer([FromBody] OfferModel offerModel)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		_offerService.Add(offerModel);
		return Ok();
	}

	[HttpPost("update")]
	public IActionResult UpdateOffer([FromBody] OfferModel offerModel)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		_offerService.Update(offerModel);
		return Ok();
	}

	[HttpDelete("delete")]
	public IActionResult DeleteOffer([FromQuery] Guid id)
	{
		_offerService.Delete(id);
		return Ok();
	}

	[HttpGet("get")]
	[ProducesResponseType(typeof(OfferModel), (int)HttpStatusCode.OK)]
	public IActionResult GetOffer([FromQuery] Guid id)
	{
		var offer = _offerService.GetById(id);
		return Ok(offer);
	}

	[HttpGet("get-all-by-order")]
	[ProducesResponseType(typeof(List<OfferModel>), (int)HttpStatusCode.OK)]
	public IActionResult GetOffersByOrder([FromQuery] Guid orderId)
	{
		var orderOffers = _offerService.GetAllByOrderId(orderId);
		return Ok(orderOffers);
	}

	[HttpGet("get-all-by-carrier")]
	[ProducesResponseType(typeof(List<OfferModel>), (int)HttpStatusCode.OK)]
	public IActionResult GetOffersByCarrier([FromQuery] Guid carrierId)
	{
		var carrierOffers = _offerService.GetAllByCarrierId(carrierId);
		return Ok(carrierOffers);
	}
}
