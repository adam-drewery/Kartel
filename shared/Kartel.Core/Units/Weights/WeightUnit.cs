﻿using System;

namespace Kartel.Units.Weights;

public class WeightUnit : Unit
{
    public static WeightUnit Microgram { get; } = Unit("Micrograms", d => d / 1000000000);
        
    public static WeightUnit Gram { get; } = Unit("Grams", d => d / 1000);
        
    public static WeightUnit Kilogram { get; } = Unit("Kilograms", d => d);
        
    public static WeightUnit Ounce { get; } = Unit("Ounces", d => d * 0.02834952);
        
    private WeightUnit(string name, Func<double, double> toKilograms) : base(name) => ToKilograms = toKilograms;

    private static WeightUnit Unit(string name, Func<double, double> toKilograms) => new(name, toKilograms);
        
    public Func<double, double> ToKilograms { get; }
}