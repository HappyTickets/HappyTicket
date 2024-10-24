using Client.Components.Dialogs;
using Client.Services.Interfaces.UI;
using MudBlazor;
using Shared.ResourceFiles;

namespace Client.Services.Implementation.UI
{
    public class PopUpService : IPopUpService
    {
        private readonly IDialogService _dialogService;

        public PopUpService(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        public async Task<bool> ConfirmDeletionAsync()
        {
            var parameters = new DialogParameters<AlertDialog>
            {
                { x => x.ContentText, Resource.Confirm_Deletion_Text },
                { x => x.ButtonText, Resource.Delete },
                { x => x.Color, Color.Error }
            };

            var options = new DialogOptions() { CloseButton = false, MaxWidth = MaxWidth.Small, FullWidth = true };

            var dialog = await _dialogService.ShowAsync<AlertDialog>(Resource.Delete, parameters, options);

            return !(await dialog.Result).Canceled;
        }
    }
}
