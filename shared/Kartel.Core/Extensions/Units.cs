using Kartel.Units.Currencies;
using Kartel.Units.Volumes;
using Kartel.Units.Weights;

namespace Kartel.Extensions;

public static class Units
{
    public static Weight Kilogram(this double value) => new(WeightUnit.Kilogram, value);
    public static Weight Kilogram(this int value) => new(WeightUnit.Kilogram, value);
        
    public static Weight Gram(this double value) => new(WeightUnit.Gram, value);
    public static Weight Gram(this int value) => new(WeightUnit.Gram, value);
        
    public static Weight Microgram(this double value) => new(WeightUnit.Microgram, value);
    public static Weight Microgram(this int value) => new(WeightUnit.Microgram, value);
        
    public static Weight Ounce(this double value) => new(WeightUnit.Ounce, value);
    public static Weight Ounce(this int value) => new(WeightUnit.Ounce, value);
        
    public static Volume Liter(this double value) => new(VolumeUnit.Liter, value);
    public static Volume Liter(this int value) => new(VolumeUnit.Liter, value);
        
    public static Volume Milileter(this double value) => new(VolumeUnit.Milileter, value);
    public static Volume Milileter(this int value) => new(VolumeUnit.Milileter, value);
        
    public static CurrencyQuantity Gbp(this int value) => new(Currency.Gbp, value);
    public static CurrencyQuantity Gbp(this decimal value) => new(Currency.Gbp, value);
        
        
}