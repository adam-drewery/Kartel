using Blazored.LocalStorage;
using Kartel.Configuration;
using Kartel.Logging;
using Kartel.Web.Configuration;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Serilog;

namespace Kartel.Web;

public class Program
{
	public static async Task Main(string[] args)
	{
		Log.Logger = new KartelLogConfiguration()
			.WriteTo.BrowserConsole(outputTemplate: "%ckartel{_}color:white;background:#8c7574;border-radius:3px;padding:1px 2px;font-weight:600;{Message}{NewLine}{Exception}")
			.CreateLogger();
		
		var builder = WebAssemblyHostBuilder.CreateDefault(args);
		
		builder.Services.AddLogging(loggingBuilder =>
		{
			loggingBuilder.ClearProviders();
			loggingBuilder.AddSerilog(dispose: true);
		});

		builder.RootComponents.Add<App>("app");
		builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
		builder.Services.AddSingleton(_ => Settings.FromArgs<NetworkSettings>(args));
		builder.Services.AddSingleton<ClientSession>();
		builder.Services.AddBlazoredLocalStorage();
		builder.Services.AddHubs();
		//builder.Services.StartHubs();

		await builder.Build().RunAsync();
	}
}