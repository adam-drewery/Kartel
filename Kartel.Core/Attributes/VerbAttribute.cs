using System;

namespace Kartel.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class VerbAttribute : Attribute
{
    public VerbAttribute(string futureTense, string presentTense, string pastTense)
    {
        PresentTense = presentTense;
        PastTense = pastTense;
        FutureTense = futureTense;
    }
        
    public string FutureTense { get;  }
        
    public string PresentTense { get;  }
        
    public string PastTense { get;  }        
}