using Kartel.Api.Configuration;
using Kartel.Configuration;
using Kartel.MessagePack;
using Kartel.ServiceBase.Client;
using Serilog;

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

        var gameServices = new object[]
        {
            new PropertyMarketClient(networkSettings),
            new LogisticsClient(networkSettings),
            new GeocodingClient(networkSettings)
        };

        var game = new Game(gameServices)
        {
            Clock =
            {
                Interval = 100,
                SpeedFactor = 100
            }
        };
        game.Clock.Start();

        // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
        game.Error += (_, args) => Log.Error(args.Exception, args.Message);

        services.AddSingleton(game);
        services.AddNotifiers();

        services.AddMvc();
        services.AddSignalR()
            .AddMessagePackProtocol(options =>
        {
            options.SerializerOptions = KartelMessagePackSerializerOptions.Standard;
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
        {
            app.UseDeveloperExceptionPage();
            //app.UseWebAssemblyDebugging();
        }

        app.UseStaticFiles();
        //app.UseBlazorFrameworkFiles();
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