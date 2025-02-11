using Infrastructure.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;
public class Transport : BaseEntity
{
	[Required]
	public TransportType Type { get; set; }

	[Required]
	public string LicensePlate { get; set; }

	[Required]
	public double Capacity { get; set; }

	[Required]
	public Guid CarrierId { get; set; }

	#region Relations

	[JsonIgnore]
	public User Carrier { get; set; }

	#endregion
}
