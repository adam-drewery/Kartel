using Serilog;

namespace Kartel.Logging;

public class KartelLogConfiguration : LoggerConfiguration
{
    public KartelLogConfiguration()
    {
        WriteTo.Console();
    }
}