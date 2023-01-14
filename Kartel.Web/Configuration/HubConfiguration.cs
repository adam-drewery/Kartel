using Kartel.Web.HubClients.Base;
using Microsoft.AspNetCore.SignalR.Client;

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
					Console.WriteLine("Hub client listening: {0}", hubClientType);
				}
				catch (Exception e)
				{
					Console.WriteLine("Error encountered starting hub client with type {0}", hubClientType);
					Console.WriteLine(e);
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