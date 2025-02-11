namespace Infrastructure.Exceptions;
public class DeLogiHubException : Exception
{
	public DeLogiHubException(string? message = null)
		: base(message) { }
}
