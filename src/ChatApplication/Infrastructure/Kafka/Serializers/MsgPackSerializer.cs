namespace ChatApplication.Infrastructure.Kafka.Serializers;

using MessagePack;
using Confluent.Kafka;

public class MsgPackSerializer<T> : ISerializer<T>
{
    public byte[] Serialize(T data, SerializationContext context)
        => data == null ? new byte[1] : MessagePackSerializer.Serialize(data);
}
