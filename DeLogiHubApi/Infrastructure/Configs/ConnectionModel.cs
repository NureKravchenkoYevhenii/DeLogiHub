namespace Infrastructure.Configs;
public class ConnectionModel
{
	public string Host { get; set; } = string.Empty;

	public string Database { get; set; } = string.Empty;

	public string Password { get; set; } = string.Empty;

	public string ConnectionString { get; set; } = string.Empty;
}
