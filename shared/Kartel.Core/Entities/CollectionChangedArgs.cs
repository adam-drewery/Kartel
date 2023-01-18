namespace Kartel.Entities;

public class CollectionChangedArgs
{
    public CollectionChangedArgs(object item)
    {
        Item = item;
    }
    
    public object Item { get; }
}