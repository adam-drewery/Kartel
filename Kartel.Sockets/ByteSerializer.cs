using MessagePack;

namespace Kartel.Sockets;

public static class ByteSerializer
{
	private static readonly MessagePackSerializerOptions Options = MessagePack.Resolvers.ContractlessStandardResolver.Options;

	public static byte[] Serialize(object source) => MessagePackSerializer.Serialize(source, Options);

	public static T Deserialize<T>(byte[] bytes) => MessagePackSerializer.Deserialize<T>(bytes, Options);
}