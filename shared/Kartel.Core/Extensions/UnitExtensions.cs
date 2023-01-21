using Kartel.Units.Currencies;
using Kartel.Units.Volumes;
using Kartel.Units.Weights;

namespace Kartel.Extensions;

public static class UnitExtensions
{
    public static Weight Kilograms(this double value) => new(WeightUnit.Kilogram, value);
    public static Weight Kilograms(this int value) => new(WeightUnit.Kilogram, value);
        
    public static Weight Grams(this double value) => new(WeightUnit.Gram, value);
    public static Weight Grams(this int value) => new(WeightUnit.Gram, value);
        
    public static Weight Micrograms(this double value) => new(WeightUnit.Microgram, value);
    public static Weight Micrograms(this int value) => new(WeightUnit.Microgram, value);
        
    public static Weight Ounces(this double value) => new(WeightUnit.Ounce, value);
    public static Weight Ounces(this int value) => new(WeightUnit.Ounce, value);
        
    public static Volume Liters(this double value) => new(VolumeUnit.Liter, value);
    public static Volume Liters(this int value) => new(VolumeUnit.Liter, value);
        
    public static Volume Milliliters(this double value) => new(VolumeUnit.Milliliter, value);
    public static Volume Milliliters(this int value) => new(VolumeUnit.Milliliter, value);
        
    public static Volume CubicFeet(this double value) => new(VolumeUnit.CubicFoot, value);
    public static Volume CubicFeet(this int value) => new(VolumeUnit.CubicFoot, value);
        
    public static CurrencyQuantity Gbp(this int value) => new(Currency.Gbp, value);
    public static CurrencyQuantity Gbp(this decimal value) => new(Currency.Gbp, value);
        
        
}