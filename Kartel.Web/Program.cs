using Blazored.LocalStorage;
using Kartel.Configuration;
using Kartel.Web.Configuration;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Kartel.Web;

public class Program
{
	public static async Task Main(string[] args)
	{
		var builder = WebAssemblyHostBuilder.CreateDefault(args);

		builder.RootComponents.Add<App>("app");
		builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
		builder.Services.AddSingleton(_ => Settings.FromArgs<NetworkSettings>(args));

		foreach (var file in Directory.EnumerateFiles("/")) 
			Console.WriteLine(file);

		builder.Services.AddSingleton<ClientSession>();
		builder.Services.AddBlazoredLocalStorage();
		builder.Services.AddHubs();
		//builder.Services.StartHubs();

		await builder.Build().RunAsync();
	}
}