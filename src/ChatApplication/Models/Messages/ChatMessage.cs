namespace ChatApplication.Models.Messages;

using MessagePack;

[MessagePackObject]
public class ChatMessage
{
    [Key(0)]
    public string Id { get; set; }

    [Key(1)]
    public string UserName { get; set; }

    [Key(3)]
    public DateTime SendDate { get; set; }

    [Key(4)]
    public string Text { get; set; }
}
