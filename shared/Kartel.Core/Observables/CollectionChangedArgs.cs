namespace Kartel.Observables;

public class CollectionChangedArgs
{
    public CollectionChangedArgs(object? item)
    {
        Item = item;
    }
    
    public object? Item { get; }
}