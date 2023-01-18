using Kartel.Api.Hubs.Base;
using Kartel.Api.Hubs.Base.Interfaces;

namespace Kartel.Api.Configuration;

public static class HubConfiguration
{
	private static readonly Type[] NotifierTypes = typeof(Startup).Assembly
		.GetTypes()
		.Where(t => !t.IsAbstract)
		.Where(t => typeof(INotifier).IsAssignableFrom(t))
		.ToArray();
		
	private static readonly Type[] HubTypes = typeof(Startup).Assembly
		.GetTypes()
		.Where(t => !t.IsAbstract)
		.Where(t => typeof(BaseHub).IsAssignableFrom(t))
		.ToArray();
		
	public static void AddNotifiers(this IServiceCollection services)
	{
		foreach (var notifierType in NotifierTypes) 
			services.AddTransient(notifierType);
	}

	public static void UseNotifiers(this IApplicationBuilder app)
	{
		var serviceProvider = app.ApplicationServices;
		var game = serviceProvider.GetService<Game>();
			
		foreach (var notifierType in NotifierTypes)
		{
			var notifier = (INotifier)serviceProvider.GetService(notifierType);
			notifier.Watch(game);
		}
	}

	public static void MapHubs(this IEndpointRouteBuilder endpoints)
	{
		foreach (var hubType in HubTypes)
		{
			var method = typeof(HubEndpointRouteBuilderExtensions)
				.GetMethods()
				.Where(m => m.Name == nameof(HubEndpointRouteBuilderExtensions.MapHub))
				.Single(m => m.GetParameters().Length == 2);

			if (method == null) throw new MissingMemberException();

			var args = new object[] {endpoints, hubType.Name.ToLowerInvariant()};
			method.MakeGenericMethod(hubType).Invoke(null, args);
		}
	}
}