namespace ChatApplication.Infrastructure.Providers.DateTimeProvider;

using Application.Providers;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime DateTimeNow()
        => DateTime.UtcNow;
}
