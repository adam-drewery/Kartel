using System;
using Kartel.Units;
using Kartel.Units.Currencies;

namespace Kartel.Environment.Topography;

public class BasePrice
{
    public BasePrice(Type productType, CurrencyQuantity price, IQuantity quantity)
    {
        ProductType = productType;
        Price = price;
        Quantity = quantity;
    }
        
    public Type ProductType { get; }
        
    public CurrencyQuantity Price { get; }
        
    public IQuantity Quantity { get; }
}