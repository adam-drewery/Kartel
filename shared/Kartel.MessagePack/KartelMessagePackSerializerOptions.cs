using MessagePack;

namespace Kartel.MessagePack;

public static class KartelMessagePackSerializerOptions
{
    public static readonly MessagePackSerializerOptions Standard =
        MessagePackSerializerOptions.Standard.WithResolver(KartelFormatterResolver.DefaultResolvers);
}