namespace Kartel.Units;

public abstract class Unit
{
    protected Unit(string name) => Name = name;
        
    public string Name { get; }
}