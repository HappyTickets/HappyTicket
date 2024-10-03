using Client.Services.Interfaces.UI;
using Humanizer;
using MudBlazor;

namespace Client.Services.Implementation.UI
{
    public class CustomSnackbarProvider(ISnackbar snackbar) : ICustomSnackbarProvider
    {
        public void Show(string message, Severity severity, string position = Defaults.Classes.Position.TopRight, Action<SnackbarOptions> configurations = default!)
        {
            //snackbar.Clear();
            snackbar.Configuration.PositionClass = position;

            if (configurations == default!)
            {
                snackbar.Configuration.ClearAfterNavigation = false;
                snackbar.Configuration.NewestOnTop = true;
                snackbar.Configuration.PreventDuplicates = true;
                snackbar.Configuration.ShowCloseIcon = true;
                snackbar.Add(message, severity);
            }
            else
            {
                snackbar.Add(message, severity, configurations);
            }
        }
    }
}
