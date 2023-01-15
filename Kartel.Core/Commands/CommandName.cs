using System.Reflection;
using Kartel.Attributes;

namespace Kartel.Commands;

public class CommandName
{
    public CommandName(Command command)
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