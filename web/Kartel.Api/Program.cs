using Kartel.Logging;
using Serilog;

namespace Kartel.Api;

public class Program
{
    public static string[] Args { get; private set; } = null!;

    public static void Main(string[] args)
    {
        Args = args;
        
        Log.Logger = new KartelLogConfiguration()
            .CreateLogger();
        
        Log.Information("Logging initialized");
        
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
            .ConfigureLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddSerilog(dispose: true);
            })
            .Build()
            .Run();
    }
}