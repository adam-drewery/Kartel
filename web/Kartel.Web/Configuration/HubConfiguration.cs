using Kartel.MessagePack;
using Kartel.Web.HubClients.Base;
using MessagePack;
using Microsoft.AspNetCore.SignalR.Client;
using Serilog;

namespace Kartel.Web.Configuration;

internal static class HubConfiguration
{
    private static readonly IEnumerable<Type> HubClientTypes = typeof(Program).Assembly
        .GetTypes()
        .Where(t => !t.IsAbstract)
        .Where(t => typeof(HubClient).IsAssignableFrom(t));

    public static async Task StartHubs(this IServiceCollection services)
    {
        var provider = services.BuildServiceProvider();

        foreach (var hubClientType in HubClientTypes)
        {
            try
            {
                var hubClient = (HubClient)provider.GetService(hubClientType);

                if (hubClient == null) throw new InvalidOperationException("Couldn't resolve a HubClient type.");

                await hubClient.Connect();
                Log.Information("Hub client listening: {HubClient}", hubClientType);
            }
            catch (Exception e)
            {
                Log.Error(e, "Error encountered starting hub client with type {HubClient}", hubClientType);
            }
        }
    }

    public static void AddHubs(this IServiceCollection services)
    {
        services.AddTransient(_ => (HubConnectionBuilder)new HubConnectionBuilder()
            .WithAutomaticReconnect()
            .ConfigureLogging(builder => builder.AddSerilog())
            .AddMessagePackProtocol(options =>
            {
                // Don't pass in a game instance, we don't have one
                var resolver = new KartelFormatterResolver(null);
                options.SerializerOptions = MessagePackSerializerOptions.Standard
                    .WithResolver(resolver.CreateComposite());
            }));

        foreach (var hubClientType in HubClientTypes)
            services.AddSingleton(hubClientType);
    }
}