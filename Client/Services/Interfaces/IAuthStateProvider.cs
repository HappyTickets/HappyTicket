namespace Client.Services.Interfaces
{
    public interface IAuthStateProvider
    {
        bool IsAuthenticated { get; }
        event Action OnChange;

        void SetAuthenticationState(bool isAuthenticated);
    }
}
