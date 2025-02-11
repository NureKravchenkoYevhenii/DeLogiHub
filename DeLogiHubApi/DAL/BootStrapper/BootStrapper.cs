using Autofac;
using System.Reflection;

namespace DAL.BootStrapper;
public static class BootStrapper
{
	public static void Bootstrap(ContainerBuilder builder)
	{
		RegisterRepositories(builder);
	}

	private static void RegisterRepositories(ContainerBuilder builder)
	{
		builder.RegisterAssemblyTypes(Assembly.Load("DAL"))
			.Where(x => x.Name.EndsWith("Repository") || x.Name.Equals("UnitOfWork"))
			.AsImplementedInterfaces()
			.InstancePerLifetimeScope();
	}
}
