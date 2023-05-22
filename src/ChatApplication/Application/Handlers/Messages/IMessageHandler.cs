namespace ChatApplication.Application.Handlers.Messages;

using Models.Messages;

public interface IMessageHandler
{
    void HandleMessage(ChatMessage message);
}