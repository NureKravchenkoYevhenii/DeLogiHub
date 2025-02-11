namespace DeLogiHubApi.Infrastructure.Extensions;

public static class GuidExtensions
{
	private static readonly Random _random = new((int)(DateTime.Now.Ticks % 1900001));
	private static string Symbols => "qwertyuiopasdfghjklzxcvbnm1234567890";
	private static HashSet<int> GuidIndexes => [8, 13, 18, 23];

	public static string EncodeToken(this Guid guid)
	{
		return new string(guid.ToString()
			.Select((x, i) => GuidIndexes.Contains(i)
				? Symbols[_random.Next(0, Symbols.Length)]
				: x
			)
			.ToArray()
		);
	}

	public static Guid DecodeToken(this string guid)
	{
		return Guid.Parse(guid
			.Select((x, i) => GuidIndexes.Contains(i)
				? '-'
				: x
			)
			.ToArray()
		);
	}
}
