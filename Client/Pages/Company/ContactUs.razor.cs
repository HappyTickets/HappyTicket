using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.DTOs;

namespace Client.Pages.Company
{
    public partial class ContactUs
    {
        private CustomerInfoDto CustomerInfoDto { get; set; } = new();

        [CascadingParameter]
        public Task<AuthenticationState>? AuthenticationState { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            var user = (await AuthenticationState!).User;
        }

        private bool IsFormValidToProceedWith()
        {
            var result = !string.IsNullOrWhiteSpace(CustomerInfoDto.Name) &&
                         !string.IsNullOrWhiteSpace(CustomerInfoDto.Email) &&
                         !string.IsNullOrWhiteSpace(CustomerInfoDto.Message);

            return result;
        }

        private async Task SendCustomerRequest()
        {
            Console.WriteLine("Message Sent!");
        }
    }
}
