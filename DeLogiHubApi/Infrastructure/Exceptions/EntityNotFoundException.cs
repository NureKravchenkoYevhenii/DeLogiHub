namespace Infrastructure.Exceptions;
public class EntityNotFoundException : DeLogiHubException
{
	public EntityNotFoundException(string message)
		: base(message) { }
}

