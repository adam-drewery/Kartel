namespace Kartel.Environment.Topography;

public class Town : Location
{
    public string Name { get; set; }

    public Town(Game game, double latitude, double longitude) : base(latitude, longitude) { }
}