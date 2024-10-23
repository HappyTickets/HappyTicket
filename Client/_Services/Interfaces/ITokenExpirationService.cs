namespace Client.Services.Interfaces
{
    public interface ITokenExpirationService
    {
        Task<bool> CheckTokenExpiration();

    }
}
