using AutoMapper;
using BLL.Contracts;
using BLL.Infrastructure.Models.Offers;
using DAL.Contracts;
using Domain.Models;
using Infrastructure.Exceptions;

namespace BLL.Services;
public class OfferService : IOfferService
{
	private readonly Lazy<IUnitOfWork> _unitOfWork;
	private readonly Lazy<IMapper> _mapper;
	private readonly Lazy<IRepository<Offer>> _offers;

	public OfferService(
		Lazy<IUnitOfWork> unitOfWork,
		Lazy<IMapper> mapper)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;

		_offers = _unitOfWork.Value.GetLazyRepository<Offer>();
	}
	public void Add(OfferModel offerModel)
	{
		var offer = _mapper.Value.Map<Offer>(offerModel);

		_offers.Value.Add(offer);
	}

	public void Delete(Guid offerId)
	{
		var offer = _offers.Value.GetById(offerId);
		if (offer == null)
			return;

		_offers.Value.Remove(offer);
	}

	public List<OfferModel> GetAllByCarrierId(Guid carrierId)
	{
		var carrierOffers = _offers.Value
			.GetList(o => o.CarrierId == carrierId)
			.ToList();
		var carrierOffersModel = _mapper.Value.Map<List<OfferModel>>(carrierOffers);

		return carrierOffersModel;
	}

	public OfferModel GetById(Guid offerId)
	{
		var offer = _offers.Value.GetById(offerId)
			?? throw new EntityNotFoundException("Пропозицію не знайдено");
		var offerModel = _mapper.Value.Map<OfferModel>(offer);

		return offerModel;
	}

	public List<OfferModel> GetAllByOrderId(Guid orderId)
	{
		var orderOffers = _offers.Value
			.GetList(o => o.OrderId == orderId)
			.ToList();
		var orderOffersModel = _mapper.Value.Map<List<OfferModel>>(orderOffers);

		return orderOffersModel;
	}

	public void Update(OfferModel offerModel)
	{
		var offer = _offers.Value.GetById(offerModel.Id)
			?? throw new EntityNotFoundException("Пропозицію не знайдено");

		_mapper.Value.Map(offerModel, offer);
		_offers.Value.Update(offer);
	}
}
