using AutoMapper;

namespace DeLogiHubApi.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddMapper(
		this IServiceCollection services)
	{
		var mapperConfig = new MapperConfiguration(cfg =>
		{
			cfg.AddMaps(new[] {
				nameof(BLL),
			});
		});

		var mapper = mapperConfig.CreateMapper();
		services.AddSingleton(mapper);

		return services;
	}
}
