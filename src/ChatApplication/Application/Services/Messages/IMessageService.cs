namespace ChatApplication.Application.Services.Messages
{
    public interface IMessageService
    {
        Task SendMessage(string username, string text);
    }
}