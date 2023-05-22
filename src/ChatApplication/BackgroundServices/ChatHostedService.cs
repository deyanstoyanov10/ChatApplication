namespace ChatApplication.BackgroundServices;

using Application.Kafka;
using Application.Handlers.Messages;
using Models.Messages;

public class ChatHostedService : IHostedService
{
    private readonly IKafkaConsumer<string, ChatMessage> _kafkaConsumer;
    private readonly IMessageHandler _messageHandler;

    public ChatHostedService(
        IKafkaConsumer<string, ChatMessage> kafkaConsumer,
        IMessageHandler messageHandler) 
            => (_kafkaConsumer, _messageHandler) = (kafkaConsumer, messageHandler);

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Factory.StartNew(() =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var result = _kafkaConsumer.Consume(cancellationToken);

                    if (result?.Message?.Value is not null)
                    {
                        _messageHandler.HandleMessage(result.Message.Value);
                    }
                }
                catch (Exception ex)
                {
                    // TODO: LOG
                }
            }
        }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _kafkaConsumer?.Dispose();
        return Task.CompletedTask;
    }
}
