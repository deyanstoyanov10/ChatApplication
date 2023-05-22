namespace ChatApplication.Application.Kafka;

using Confluent.Kafka;

public interface IKafkaConsumer<TKey, TContract> : IDisposable
{
    ConsumeResult<TKey, TContract> Consume(CancellationToken cancellationToken = default);
}
