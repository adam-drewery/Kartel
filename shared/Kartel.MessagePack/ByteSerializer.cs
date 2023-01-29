using MessagePack;

namespace Kartel.MessagePack;

public class ByteSerializer
{
	private readonly MessagePackSerializerOptions _options;

	public ByteSerializer(IGame game)
	{
		_options = KartelMessagePackSerializerOptions.ForGame(game);
	}
	
	public byte[] Serialize(object source) => MessagePackSerializer.Serialize(source, _options);

	public T Deserialize<T>(byte[] bytes) => MessagePackSerializer.Deserialize<T>(bytes, _options);
}