namespace Kartel.Configuration;

public record SocketPair
{
    public string Server { get; set; }
    
    public string Client { get; set; }
}