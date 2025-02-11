using BLL.Infrastructure.Models.Offers;

namespace BLL.Contracts;
public interface IOfferService
{
	void Add(OfferModel offerModel);

	void Update(OfferModel offerModel);

	void Delete(Guid offerId);

	OfferModel GetById(Guid offerId);

	List<OfferModel> GetAllByOrderId(Guid orderId);

	List<OfferModel> GetAllByCarrierId(Guid carrierId);
}
