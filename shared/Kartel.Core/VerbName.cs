using System.IO;
using System.Reflection;
using Kartel.Attributes;

namespace Kartel;

public class VerbName
{
    public VerbName(object target)
    {
        var attribute = target.GetType()
            .GetCustomAttribute<VerbAttribute>();

        if (attribute == null)
            throw new InvalidDataException(target.GetType() + " is missing a VerbAttribute.");
        
        FutureTense = attribute.FutureTense;
        PresentTense = attribute.PresentTense;
        PastTense = attribute.PastTense;
    }

    public VerbName(string presentTense, string pastTense, string futureTense)
    {
        FutureTense = futureTense;
        PresentTense = presentTense;
        PastTense = pastTense;
    }

    public string FutureTense { get; }

    public string PresentTense { get; }

    public string PastTense { get; }
}