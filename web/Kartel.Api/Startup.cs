using Kartel.Api.Configuration;
using Kartel.Configuration;
using Kartel.MessagePack;
using Kartel.ServiceBase.Client;

namespace Kartel.Api;

public class Startup
{
    private readonly string[] _allowedOrigins =
    {
        "http://0.0.0.0:6840",
        "http://0.0.0.0:6841",
        "http://0.0.0.0:8080",
        
        "http://localhost:6840",
        "http://localhost:6841",
        "http://localhost:8080"
    };
    
    public void ConfigureServices(IServiceCollection services)
    {
        var networkSettings = Settings.FromArgs<NetworkSettings>(Program.Args);

        var game = new Game(
            g => new PropertyMarketClient(g, networkSettings),
            g => new LocaleClient(g, networkSettings),
            g => new LogisticsClient(g, networkSettings),
            g => new GeocodingClient(g, networkSettings))
        {
            Clock =
            {
                Interval = 100,
                SpeedFactor = 1000
            }
        };
        
        game.Clock.Start();
        services.AddSingleton(game);
        services.AddNotifiers();

        services.AddMvc();
        services.AddSignalR()
            .AddMessagePackProtocol(options =>
        {
            options.SerializerOptions = KartelMessagePackSerializerOptions.ForGame(game);
        });

        services.AddCors(options =>
        {
            options.AddPolicy(nameof(_allowedOrigins), builder =>
                builder
                    .WithOrigins(_allowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                );
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment()) 
            app.UseDeveloperExceptionPage();

        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors(nameof(_allowedOrigins));
        app.UseNotifiers();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
            endpoints.MapFallbackToFile("index.html");
            endpoints.MapHubs();
        });
    }
}