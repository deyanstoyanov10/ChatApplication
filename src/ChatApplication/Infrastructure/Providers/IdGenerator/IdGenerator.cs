namespace ChatApplication.Infrastructure.Providers.IdGenerator;

using Application.Providers;

public class IdGenerator : IIdGenerator
{
    public string GenerateId()
        => Guid.NewGuid().ToString().Replace("-", "");
}