using MessagePack;

namespace Kartel.MessagePack;

public static class KartelMessagePackSerializerOptions
{
    public static MessagePackSerializerOptions ForGame(IGame game)
    {
        var resolver = new KartelFormatterResolver(game);
        return MessagePackSerializerOptions.Standard.WithResolver(resolver.CreateComposite());
    }
}