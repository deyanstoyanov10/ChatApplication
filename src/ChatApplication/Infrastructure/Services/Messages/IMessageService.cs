namespace ChatApplication.Infrastructure.Services.Messages
{
    public interface IMessageService
    {
        Task SendMessage(string username, string text);
    }
}