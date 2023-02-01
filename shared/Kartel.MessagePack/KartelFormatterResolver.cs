using Kartel.Commands;
using Kartel.Entities;
using Kartel.Environment;
using Kartel.Environment.Topography;
using Kartel.EventArgs;
using Kartel.MessagePack.Formatters;
using MessagePack;
using MessagePack.Formatters;
using MessagePack.Resolvers;

namespace Kartel.MessagePack;

public class KartelFormatterResolver : IFormatterResolver
{
    private readonly IGame? _game;

    public KartelFormatterResolver(IGame? game)
    {
        _game = game;
        
        _formatterMap = new Dictionary<Type, Func<IMessagePackFormatter>>
        {
            { typeof(Person), () => new PersonFormatter(_game) },
            { typeof(Player), () => new PlayerFormatter(_game) },
            { typeof(Location), () => new LocationFormatter(_game) },
            { typeof(House), () => new HouseFormatter(_game) },
            { typeof(Shop), () => new ShopFormatter(_game) },
            { typeof(Relationship), () => new RelationshipFormatter(_game) },
            { typeof(PropertyChangedArgs), () => new PropertyChangedArgsFormatter(_game) }
        };
        
        // add various command formatters via reflection
        var commandFormatter = new CommandFormatter();
        var commandTypes = typeof(Command).Assembly
            .GetTypes()
            .Where(t => !t.IsAbstract)
            .Where(t => t.IsAssignableTo(typeof(Command)));
        
        foreach (var type in commandTypes) 
            _formatterMap.Add(type, () => commandFormatter);
    }
    
    private readonly Dictionary<Type, Func<IMessagePackFormatter>> _formatterMap;

    private IMessagePackFormatter? GetFormatter(Type t)
    {
        // If type can not get, must return null for fallback mechanism.
        return _formatterMap.TryGetValue(t, out var formatter) ? formatter() : null;
    }
    
    public IMessagePackFormatter<T>? GetFormatter<T>() => (IMessagePackFormatter<T>?)GetFormatter(typeof(T));

    public IFormatterResolver CreateComposite() => CompositeResolver.Create(
        new KartelFormatterResolver(_game),
        ContractlessStandardResolverAllowPrivate.Instance
    );
}