namespace ChatApplication.Infrastructure.Kafka.Consumers;

using Application.Kafka;
using Application.Providers;
using Configurations;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

public class KafkaConsumer<TKey, TContract> : IKafkaConsumer<TKey, TContract> where TContract : class
{
    private readonly IConsumer<TKey, TContract> _consumer;
    private readonly IIdGenerator _idGenerator;

    public KafkaConsumer(
        IIdGenerator idGenerator,
        IOptionsMonitor<ConsumerConfiguration> consumerConfiguration,
        IDeserializer<TKey> keyDeserializer,
        IDeserializer<TContract> valueDeserializer)
    {
        _idGenerator = idGenerator;
        var consumerConfig = consumerConfiguration.CurrentValue;
        consumerConfig.GroupId = _idGenerator.GenerateId();

        _consumer = new ConsumerBuilder<TKey, TContract>(consumerConfig)
            .SetKeyDeserializer(keyDeserializer)
            .SetValueDeserializer(valueDeserializer)
            .Build();

        _consumer.Subscribe(consumerConfiguration.CurrentValue.MessageTopic);
    }

    public ConsumeResult<TKey, TContract> Consume(CancellationToken cancellationToken = default)
        => _consumer.Consume(cancellationToken);

    public void Dispose()
        => _consumer.Dispose();
}
