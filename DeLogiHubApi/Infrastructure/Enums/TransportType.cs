using System.Text.Json.Serialization;

namespace Infrastructure.Enums;
[JsonConverter(typeof(JsonStringEnumConverter<TransportType>))]
public enum TransportType
{
	Unknown,
	None,
	Truck,
	Van,
	Refrigerated,
	Tanker,
	Container
}
