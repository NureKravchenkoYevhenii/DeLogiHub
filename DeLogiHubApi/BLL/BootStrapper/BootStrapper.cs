using Autofac;
using System.Reflection;

namespace BLL.BootStrapper;
public class BootStrapper
{
	public static void Bootstrap(ContainerBuilder builder)
	{
		DAL.BootStrapper.BootStrapper.Bootstrap(builder);
		RegisterServices(builder);
	}

	private static void RegisterServices(ContainerBuilder builder)
	{
		builder.RegisterAssemblyTypes(Assembly.Load("BLL"))
			.Where(x => x.Name.EndsWith("Service"))
			.AsImplementedInterfaces()
			.InstancePerLifetimeScope();
	}
}
