namespace Kartel.Entities.Items.Containers;

public class Inventory : Container
{
    private readonly Person _person;
        
    public Inventory() { }
        
    public Inventory(Person person) => _person = person;

    public override double Capacity => 0;

    public override double BaseWeight { get; } = 0;
}