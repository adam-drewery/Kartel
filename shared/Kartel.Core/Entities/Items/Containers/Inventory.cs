using Kartel.Extensions;
using Kartel.Units.Currencies;

namespace Kartel.Entities.Items.Containers;

public class Inventory : Container
{
    private readonly Person _person;
        
    public Inventory(Person person) : base(20.Liters(), 0.Grams()) 
        => _person = person;

    public override CurrencyQuantity BasePrice { get; } = 0.Gbp();
}