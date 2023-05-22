namespace ChatApplication.Infrastructure.Kafka.Producers;

using Confluent.Kafka;
using Microsoft.Extensions.Options;

public class KafkaProducer<TKey, TValue> : IKafkaProducer<TKey, TValue>
{
    private readonly IProducer<TKey, TValue> _producer;
    private readonly ILogger<KafkaProducer<TKey, TValue>> _logger;

    public KafkaProducer(
        ILogger<KafkaProducer<TKey, TValue>> logger,
        ISerializer<TKey> keySerializer,
        ISerializer<TValue> valueSerializer,
        IOptionsMonitor<ProducerConfig> producerConfig)
    {
        _logger = logger;

        var producerConfiguration = producerConfig?.CurrentValue ?? throw new ArgumentNullException("Missing producer configuration");
        _producer = new ProducerBuilder<TKey, TValue>(producerConfiguration)
            .SetKeySerializer(keySerializer)
            .SetValueSerializer(valueSerializer)
            .Build();
    }

    public async Task ProduceAsync(string topic, Message<TKey, TValue> message)
    {
        try
        {
            var result = await _producer.ProduceAsync(topic, message);
            _logger.LogDebug($"Message delivered '{result.Value}' to '{result.TopicPartitionOffset}'");
        }
        catch (ProduceException<TKey, TValue> ex)
        {
            _logger.LogError($"Delivery failed: {ex.Error.Reason}");
        }
    }

    public void Dispose()
        => _producer.Dispose();
}