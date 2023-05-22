namespace ChatApplication.Infrastructure.Services.Providers.IdGenerator;

public class IdGenerator : IIdGenerator
{
    public string GenerateId()
        => Guid.NewGuid().ToString().Replace("-", "");
}
