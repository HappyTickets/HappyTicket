using Client.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.DTOs;
using Shared.DTOs.ContactDto;

namespace Client.Pages.Company
{
    public partial class ContactUs
    {
        private CustomerInfoDto CustomerInfoDto { get; set; } = new();

        [Inject]
        private BIContactService ContactService { get; set; } = null!; // Inject the contact service

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
                try
                {
                    await ContactService.SendMessageAsync(new Contact
                    {
                        Username = CustomerInfoDto.Name,
                        Email = CustomerInfoDto.Email,
                        Note = CustomerInfoDto.Message
                    });

                    Console.WriteLine("Message Sent!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending message: {ex.Message}");
                }
            }
        }
    }
}
