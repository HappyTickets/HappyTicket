namespace Application.Interfaces.Infrastructure.Services
{
    public interface ICurrentUser
    {
        long? Id { get; }
        string? Name { get; }
        string? Email { get; }
    }
}
