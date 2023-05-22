namespace ChatApplication.Infrastructure.Kafka.Producers;

using Confluent.Kafka;

public interface IKafkaProducer<TKey, TValue> : IDisposable
{
    Task ProduceAsync(string topic, Message<TKey, TValue> message);
}
