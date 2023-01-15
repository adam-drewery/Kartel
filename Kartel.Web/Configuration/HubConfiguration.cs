using Kartel.Web.HubClients.Base;
using Microsoft.AspNetCore.SignalR.Client;
using Serilog;

namespace Kartel.Web.Configuration;

internal static class HubConfiguration
{
	private static readonly IEnumerable<Type> HubClientTypes = typeof(Program).Assembly
		.GetTypes()
		.Where(t => !t.IsAbstract)
		.Where(t => typeof(HubClient).IsAssignableFrom(t));

	public static void StartHubs(this IServiceCollection services)
	{
		var provider = services.BuildServiceProvider();

		Task.Run(async () =>
		{
			foreach (var hubClientType in HubClientTypes)
			{
				try
				{
					var hubClient = (HubClient) provider.GetService(hubClientType);

					if (hubClient == null) throw new InvalidOperationException("Couldn't resolve a HubClient type.");
						
					await hubClient.Connect();
					Log.Information("Hub client listening: {HubClient}", hubClientType);
				}
				catch (Exception e)
				{
					Log.Error(e, "Error encountered starting hub client with type {HubClient}", hubClientType);
				}
			}
		});
	}

	public static void AddHubs(this IServiceCollection services)
	{
		services.AddTransient<HubConnectionBuilder>();

		foreach (var hubClientType in HubClientTypes)
			services.AddSingleton(hubClientType);
	}
}