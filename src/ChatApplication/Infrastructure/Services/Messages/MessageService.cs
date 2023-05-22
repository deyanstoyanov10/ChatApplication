namespace ChatApplication.Infrastructure.Services.Messages;

using Confluent.Kafka;
using Configurations;
using System.Web;
using Models.Messages;
using Application.Kafka;
using Application.Providers;
using Application.Services.Messages;
using Microsoft.Extensions.Options;

public class MessageService : IMessageService
{
    private readonly IKafkaProducer<string, ChatMessage> _producer;
    private readonly IIdGenerator _idGenerator;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly MessageLoggingConfiguration _messageLoggingConfig;

    public MessageService(
        IKafkaProducer<string, ChatMessage> producer,
        IOptionsMonitor<MessageLoggingConfiguration> messageLoggingConfig,
        IIdGenerator idGenerator,
        IDateTimeProvider dateTimeProvider)
    {
        _producer = producer;
        _idGenerator = idGenerator;
        _dateTimeProvider = dateTimeProvider;
        _messageLoggingConfig = messageLoggingConfig?.CurrentValue ?? throw new ArgumentNullException("Kafka message logging not configured.");
    }

    public async Task SendMessage(string username, string text)
    {
        var msg = BuildMessage(username, text);

        if (_messageLoggingConfig.IsEnabled)
        {
            var message = new Message<string, ChatMessage>()
            {
                Key = msg.Id,
                Value = msg
            };

            await _producer.ProduceAsync(_messageLoggingConfig.Topic, message);
        }
    }

    private ChatMessage BuildMessage(string username, string text)
    {
        var message = new ChatMessage()
        {
            Id = _idGenerator.GenerateId(),
            UserName = username,
            SendDate = _dateTimeProvider.DateTimeNow(),
            Text = HtmlEscape(text)
        };

        return message;
    }

    private string HtmlEscape(string text)
    {
        string encodedString = HttpUtility.HtmlEncode(text);

        StringWriter stringWriter = new StringWriter();

        HttpUtility.HtmlDecode(encodedString, stringWriter);

        string decodedString = stringWriter.ToString();

        return decodedString;
    }
}
