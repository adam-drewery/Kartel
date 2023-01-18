using MessagePack;
using MessagePack.Formatters;
using MessagePack.Resolvers;

namespace Kartel.MessagePack;

public class KartelFormatterResolver : IFormatterResolver
{
    public static readonly IFormatterResolver Instance = new KartelFormatterResolver();

    public IMessagePackFormatter<T> GetFormatter<T>()
    {
        return FormatterCache<T>.Formatter;
    }

    private static class FormatterCache<T>
    {
        public static readonly IMessagePackFormatter<T> Formatter = (IMessagePackFormatter<T>)CustomResolverGetFormatterHelper.GetFormatter(typeof(T));
    }
    
    public static readonly IFormatterResolver DefaultResolvers = CompositeResolver.Create(
        Instance,
        ContractlessStandardResolverAllowPrivate.Instance
    );
}