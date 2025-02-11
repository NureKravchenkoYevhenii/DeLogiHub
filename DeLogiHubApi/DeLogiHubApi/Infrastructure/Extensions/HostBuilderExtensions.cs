using Autofac;
using Autofac.Extensions.DependencyInjection;
using Infrastructure.IoC;

namespace DeLogiHubApi.Infrastructure.Extensions;

public static class HostBuilderExtensions
{
	public static IHostBuilder ConfigureAutofac(this IHostBuilder host)
	{
		host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
		host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
		{
			containerBuilder.RegisterBuildCallback(ctx =>
			{
				IoC.Container = ctx.Resolve<ILifetimeScope>();
			});

			BLL.BootStrapper.BootStrapper.Bootstrap(containerBuilder);
		});

		return host;
	}
}
