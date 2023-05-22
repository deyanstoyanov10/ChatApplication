namespace ChatApplication.Infrastructure.Configurations;

public class PushClientConfiguration
{
    public TimeSpan PublishInterval { get; set; }

    public int MaxSize { get; set; }
}
