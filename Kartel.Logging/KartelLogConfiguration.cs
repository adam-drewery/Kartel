using Serilog;

namespace Kartel.Logging;

public class KartelLogConfiguration : LoggerConfiguration
{
    public KartelLogConfiguration()
    {
        MinimumLevel.Verbose().WriteTo.Console();
    }
}