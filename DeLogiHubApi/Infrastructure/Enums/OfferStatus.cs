using System.Text.Json.Serialization;

namespace Infrastructure.Enums;
[JsonConverter(typeof(JsonStringEnumConverter<OfferStatus>))]
public enum OfferStatus
{
	Unknown,
	Pending,
	Accepted,
	Rejected
}
