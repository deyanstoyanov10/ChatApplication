namespace ChatApplication.Infrastructure.Services.Messages;

using Models;

public interface IMessageHandler
{
    void HandleMessage(ChatMessage message);
}
