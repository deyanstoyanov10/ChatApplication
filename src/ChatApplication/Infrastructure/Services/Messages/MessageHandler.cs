namespace ChatApplication.Infrastructure.Services.Messages;

using Models;
using Configurations;
using ChatApplication.Hub;
using Models.Message.Response;
using Microsoft.Extensions.Options;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.AspNetCore.SignalR;

public class MessageHandler : IMessageHandler
{
    private readonly Subject<ChatMessage> _eventStream;
    private readonly IOptionsMonitor<PushClientConfiguration> _pushConfiguration;
    private readonly IHubContext<ChatHub> _hubContext;

    public MessageHandler(
        IOptionsMonitor<PushClientConfiguration> pushConfiguration,
        IHubContext<ChatHub> hubContext)
    {
        _eventStream = new Subject<ChatMessage>();
        _pushConfiguration = pushConfiguration;
        _hubContext = hubContext;
        this.Subscribe();
    }

    public void HandleMessage(ChatMessage message)
        => _eventStream.OnNext(message);

    private void Subscribe()
    {
        _eventStream
            .Buffer(_pushConfiguration.CurrentValue.PublishInterval, _pushConfiguration.CurrentValue.MaxSize)
            .Subscribe(async messages =>
            {
                if (messages is not null && messages.Count > 0)
                {
                    var response = new ResponseMessages() { Data = messages };

                    await _hubContext.Clients.All.SendAsync("ReceiveMessages", response);
                }
            });
    }
}
