namespace ChatApplication.Infrastructure.Services.Providers.DateTimeProvider
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime DateTimeNow()
            => DateTime.UtcNow;
    }
}
