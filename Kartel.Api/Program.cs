using Kartel.Logging;
using Serilog;

namespace Kartel.Api;

public class Program
{
    public static string[] Args { get; private set; }
    
    public static void Main(string[] args)
    {
        Args = args;
        
        Log.Logger = new KartelLogConfiguration()
            .CreateLogger();
        
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
            .Build()
            .Run();
    }
}