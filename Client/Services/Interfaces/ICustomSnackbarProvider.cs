
using MudBlazor;

namespace Client.Services.Interfaces
{
    public interface ICustomSnackbarProvider
    {
        void Show(string message, Severity severity, string position = Defaults.Classes.Position.TopRight, Action<SnackbarOptions> configurations = default!);
    }
}
