using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Client.Components.Dialogs
{
    public partial class BaseFormDialog<T> where T : class
    {
        private MudForm _form = new();

        [Parameter]
        public bool IsVisible { get; set; }
        [Parameter]
        public EventCallback<bool> IsVisibleChanged { get; set; }
        [Parameter]
        public T Entity { get; set; } = default!;
        [Parameter]
        public EventCallback<T> EntityChanged { get; set; }

        [Parameter]
        public EventCallback<T> Submit { get; set; }
        [Parameter]
        public EventCallback<T>? Reset { get; set; }
        [Parameter]
        public EventCallback<T>? Cancel { get; set; }

        [Parameter]
        public DialogOptions DialogOptions { get; set; } = new() { FullWidth = true };
        [Parameter]
        public string ClassContent { get; set; } = "mt-n5";
        [Parameter]
        public string ContentStyle { get; set; } = string.Empty;
        [Parameter]
        public string Style { get; set; } = string.Empty;
        [Parameter]
        public bool DisableSidePadding { get; set; } = true;
        [Parameter]
        public string Title { get; set; } = string.Empty;
        [Parameter]
        public string Icon { get; set; } = string.Empty;

        [Parameter]
        public RenderFragment<T>? ChildContent { get; set; }
        [Parameter]
        public RenderFragment<T>? TitleContent { get; set; }
        [Parameter]
        public RenderFragment<T>? ActionContent { get; set; }

        private async Task SubmitClicked()
        {
            await _form!.Validate();

            if (_form.IsValid)
            {
                if (Submit.HasDelegate) await Submit.InvokeAsync(Entity);
                IsVisible = false;
                if (IsVisibleChanged.HasDelegate) await IsVisibleChanged.InvokeAsync(IsVisible);
            }
        }
        private async Task ResetClicked()
        {
            if (Reset != null && Reset.Value.HasDelegate) await Reset.Value.InvokeAsync(Entity);
        }
        private async Task CancelClicked()
        {
            if (Cancel != null && Cancel.Value.HasDelegate) await Cancel.Value.InvokeAsync(Entity);
            IsVisible = false;
            if (IsVisibleChanged.HasDelegate) await IsVisibleChanged.InvokeAsync(IsVisible);
        }
    }
}
