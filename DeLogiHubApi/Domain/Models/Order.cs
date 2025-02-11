using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;
public class Order : BaseEntity
{
	[Required]
	public Guid CustomerId { get; set; }

	public DateTime CreatedOn { get; set; }

	#region Relations

	[JsonIgnore]
	public User Customer { get; set; }

	[JsonIgnore]
	public ICollection<Offer> Offers { get; set; }

	#endregion
}