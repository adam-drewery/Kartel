using System.Reflection;
using Kartel.Attributes;

namespace Kartel.Commands;

public class ActivityName
{
    public ActivityName(Command command)
    {
        var attribute = command.GetType()
            .GetCustomAttribute<VerbAttribute>();
            
        FutureTense = attribute.FutureTense;
        PresentTense = attribute.PresentTense;
        PastTense = attribute.PastTense;
            
    }
        
    public string FutureTense { get; }

    public string PresentTense { get; }

    public string PastTense { get; }
}