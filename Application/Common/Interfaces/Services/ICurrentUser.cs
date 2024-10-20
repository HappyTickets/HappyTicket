namespace Application.Common.Interfaces.Services
{
    public interface ICurrentUser
    {
        long? Id { get; }
        string? Name { get; }
        string? Email { get; }
    }
}
