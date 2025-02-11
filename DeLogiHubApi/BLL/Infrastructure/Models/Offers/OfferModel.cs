using Infrastructure.Enums;

namespace BLL.Infrastructure.Models.Offers;
public class OfferModel
{
	public Guid Id { get; set; }

	public Guid CarrierId { get; set; }

	public Guid OrderId { get; set; }

	public OfferStatus Status { get; set; }
}
