namespace ChatApplication.Infrastructure.Kafka.Consumers;

using Confluent.Kafka;

public interface IKafkaConsumer<TKey, TContract> : IDisposable
{
    ConsumeResult<TKey, TContract> Consume(CancellationToken cancellationToken = default);
}
