namespace ChatApplication.Infrastructure.Kafka.Deserializers;

using MessagePack;
using Confluent.Kafka;

public class MsgPackDeserializer<T> : IDeserializer<T>
{
    public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        => data == null ? default : MessagePackSerializer.Deserialize<T>(data.ToArray());
}
