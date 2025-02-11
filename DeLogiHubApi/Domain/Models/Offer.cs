using Infrastructure.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;
public class Offer : BaseEntity
{
	[Required]
	public Guid CarrierId { get; set; }

	[Required]
	public Guid OrderId { get; set; }

	[Required]
	public OfferStatus Status { get; set; }

	#region Relations

	[JsonIgnore]
	public User Carrier { get; set; }

	[JsonIgnore]
	public Order Order { get; set; }

	#endregion
}
