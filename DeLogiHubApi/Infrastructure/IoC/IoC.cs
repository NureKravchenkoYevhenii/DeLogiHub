using Autofac;

namespace Infrastructure.IoC;
public class IoC
{
	public static ILifetimeScope Container { private get; set; } = null!;
}
