using System.Text.Json.Serialization;

namespace Infrastructure.Enums;
[JsonConverter(typeof(JsonStringEnumConverter<Role>))]
public enum Role
{
	Customer = 1,
	Carrier
}
