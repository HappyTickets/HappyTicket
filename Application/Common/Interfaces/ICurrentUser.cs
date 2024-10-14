namespace Application.Common.Interfaces
{
    public interface ICurrentUser
    {
        long? Id { get; }
        string? Name { get; }
        string? Email { get; }
    }
}
