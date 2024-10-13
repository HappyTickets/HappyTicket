using Client.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Shared.DTOs;
using Shared.DTOs.ContactDto;

namespace Client.Pages.Company
{
    public partial class ContactUs
    {
        private CustomerInfoDto CustomerInfoDto { get; set; } = new();

        [Inject]
        private BIContactService ContactService { get; set; } = null!;

        [CascadingParameter]
        public Task<AuthenticationState>? AuthenticationState { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            var user = (await AuthenticationState!).User;
        }

        private bool IsFormValidToProceedWith()
        {
            return !string.IsNullOrWhiteSpace(CustomerInfoDto.Name) &&
                   !string.IsNullOrWhiteSpace(CustomerInfoDto.Email) &&
                   !string.IsNullOrWhiteSpace(CustomerInfoDto.Message);
        }

        private async Task SendCustomerRequest()
        {
            if (IsFormValidToProceedWith())
            {
                var result = await ContactService.SendMessageAsync(new Contact
                {
                    Username = CustomerInfoDto.Name,
                    Email = CustomerInfoDto.Email,
                    Note = CustomerInfoDto.Message
                });
                if (result != null)
                {
                    Snackbar.Add("Failed To Send The Message Please Try Again", Severity.Error);
                }
                else
                {
                    Snackbar.Add("Message Sent Successfully", Severity.Success);
                    Navigation.NavigateTo("/");
                }
            }
        }
    }
}
