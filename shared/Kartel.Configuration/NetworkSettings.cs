namespace Kartel.Configuration;

public class NetworkSettings : Settings
{
    public SocketPair PropertyMarket { get; set; }

    public SocketPair Logistics { get; set; }

    public SocketPair Geocoding { get; set; }

    public SocketPair ReverseGeocoding { get; set; }
    
    public SocketPair Locale { get; set; }
}