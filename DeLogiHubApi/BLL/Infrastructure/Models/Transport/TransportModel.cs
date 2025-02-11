using Infrastructure.Enums;

namespace BLL.Infrastructure.Models;
public class TransportModel
{
	public Guid Id { get; set; }

	public TransportType Type { get; set; }

	public string LicensePlate { get; set; } = null!;

	public double Capacity { get; set; }

	public Guid CarrierId { get; set; }
}
