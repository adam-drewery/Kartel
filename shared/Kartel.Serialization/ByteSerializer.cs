using Kartel.MessagePack;
using MessagePack;

namespace Kartel.Serialization;

public static class ByteSerializer
{
	public static byte[] Serialize(object source) => MessagePackSerializer.Serialize(
		source, 
		KartelMessagePackSerializerOptions.Standard);

	public static T Deserialize<T>(byte[] bytes) => MessagePackSerializer.Deserialize<T>(
		bytes, 
		KartelMessagePackSerializerOptions.Standard);
}