﻿using Client.Components.Dialogs;
using Humanizer;
using LanguageExt;
using MudBlazor;
using Shared.DTOs;
using Shared.DTOs.Identity.Register;
using Shared.ResourceFiles;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Client.Pages.Identity
{
    public partial class Register
    {
        private RegisterRequest registerRequest = new RegisterRequest();
        private string CurrentPhoneNumberRegex { get; set; } = string.Empty;
        private bool IsPhoneNumberRegexErrorShown { get; set; }

        private InputType passwordInputType = InputType.Password;

        private InputType confirmingPasswordInputType = InputType.Password;

        private string passwordIcon = Icons.Material.Filled.VisibilityOff;

        private string confirmingPasswordIcon = Icons.Material.Filled.VisibilityOff;
        private List<CountryDetailsDto> Countries { get; set; } = new List<CountryDetailsDto>();

        private bool TermsAndConditionsAccepted { get; set; }
        private string SelectedCountryPrefix { get; set; }
        private string SearchText { get; set; } = string.Empty;
        private List<CountryDetailsDto> FilteredCountries => Countries
        .Where(country => string.IsNullOrEmpty(SearchText) ||
                          country.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                          country.Prefix.Contains(SearchText)).ToList();
        private void OnSearchTextChanged(string searchText)
        {
            SearchText = searchText;
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            await GetCountriesDetailsAsync();
        }

        private async Task RegisterUser()
        {
            if (!IsFormValidToProceedWith()) return;
            try
            {
                var clonedRegisterRequest = JsonSerializer.Deserialize<RegisterRequest>(JsonSerializer.Serialize(registerRequest));

                clonedRegisterRequest!.PhoneNumber = $"{SelectedCountryPrefix}{registerRequest.PhoneNumber}";

                var response = await IdentityService.RegisterAsync(clonedRegisterRequest);


                await response.Match(
                    async Succ =>
                    {
                        if (Succ.IsSuccess)
                        {
                            await LocalStorage.SetItemAsStringAsync("email", registerRequest.Email);
                            Snackbar.Show($"{Resource.Registration_Success}!", Severity.Success);
                            Snackbar.Show(Resource.EmailVerificationMessage, Severity.Info);
                            Snackbar.Show(Resource.Login_Redirect, Severity.Normal);
                            Navigation.NavigateTo("/login");
                        }
                        else
                        {
                            Snackbar.Show((string.IsNullOrWhiteSpace(Succ.Title)) ? Resource.Registration_Fail : $"{Resource.Registration_Fail}\n{Succ.Title.Humanize()}", Severity.Error);
                            Succ.ErrorList?.ToList().ForEach(err => Snackbar.Show($"{err.Title.Humanize()}: {err.Message.Humanize()}", Severity.Error));
                        }
                        return new Unit();
                    },
                    async Fail =>
                    {
                        Snackbar.Show($"{Resource.Registration_Fail}\n{Fail.Message.Humanize()}", Severity.Error);
                        await Task.Delay(0);
                        return new Unit();
                    }
                );
            }
            catch (Exception ex)
            {
                Snackbar.Show(Resource.Error_Occurred, Severity.Error);
            }
        }

        private bool IsFormValidToProceedWith()
        {
            var result = !string.IsNullOrWhiteSpace(registerRequest.UserName) &&
                         !string.IsNullOrWhiteSpace(registerRequest.Email) &&
                         !string.IsNullOrWhiteSpace(registerRequest.Password) &&
                         !string.IsNullOrWhiteSpace(registerRequest.ConfirmPassword) &&
                         !string.IsNullOrWhiteSpace(registerRequest.PhoneNumber) &&
                         !IsPhoneNumberRegexErrorShown &&
                         !string.IsNullOrWhiteSpace(SelectedCountryPrefix) &&
                         TermsAndConditionsAccepted;

            return result;
        }

        private async Task GetCountriesDetailsAsync()
        {

            var stringifiedCountriesDetails = "[{\"name\":\"Algeria\",\"prefix\":\"+213\",\"flagLinkSegment\":\"7/77/Flag_of_Algeria.svg\",\"phoneNumberRegex\":\"^([5-7])\\\\d{8}$\"},{\"name\":\"Bahrain\",\"prefix\":\"+973\",\"flagLinkSegment\":\"2/2c/Flag_of_Bahrain.svg\",\"phoneNumberRegex\":\"^3\\\\d{7}$\"},{\"name\":\"Comoros\",\"prefix\":\"+269\",\"flagLinkSegment\":\"9/94/Flag_of_the_Comoros.svg\",\"phoneNumberRegex\":\"^77\\\\d{6}$\"},{\"name\":\"Djibouti\",\"prefix\":\"+253\",\"flagLinkSegment\":\"3/34/Flag_of_Djibouti.svg\",\"phoneNumberRegex\":\"^77\\\\d{6}$\"},{\"name\":\"Egypt\",\"prefix\":\"+20\",\"flagLinkSegment\":\"f/fe/Flag_of_Egypt.svg\",\"phoneNumberRegex\":\"^01[0125]\\\\d{8}$\"},{\"name\":\"Iraq\",\"prefix\":\"+964\",\"flagLinkSegment\":\"f/f6/Flag_of_Iraq.svg\",\"phoneNumberRegex\":\"^7[0-9]\\\\d{8}$\"},{\"name\":\"Jordan\",\"prefix\":\"+962\",\"flagLinkSegment\":\"c/c0/Flag_of_Jordan.svg\",\"phoneNumberRegex\":\"^7[789]\\\\d{7}$\"},{\"name\":\"Kuwait\",\"prefix\":\"+965\",\"flagLinkSegment\":\"a/aa/Flag_of_Kuwait.svg\",\"phoneNumberRegex\":\"^([569])\\\\d{7}$\"},{\"name\":\"Lebanon\",\"prefix\":\"+961\",\"flagLinkSegment\":\"5/59/Flag_of_Lebanon.svg\",\"phoneNumberRegex\":\"^7\\\\d{7}$\"},{\"name\":\"Libya\",\"prefix\":\"+218\",\"flagLinkSegment\":\"0/05/Flag_of_Libya.svg\",\"phoneNumberRegex\":\"^9[1-9]\\\\d{7}$\"},{\"name\":\"Mauritania\",\"prefix\":\"+222\",\"flagLinkSegment\":\"4/43/Flag_of_Mauritania.svg\",\"phoneNumberRegex\":\"^[47]\\\\d{7}$\"},{\"name\":\"Morocco\",\"prefix\":\"+212\",\"flagLinkSegment\":\"2/2c/Flag_of_Morocco.svg\",\"phoneNumberRegex\":\"^6[0-9]\\\\d{8}$\"},{\"name\":\"Oman\",\"prefix\":\"+968\",\"flagLinkSegment\":\"d/dd/Flag_of_Oman.svg\",\"phoneNumberRegex\":\"^9[0-9]\\\\d{6}$\"},{\"name\":\"Palestine\",\"prefix\":\"+970\",\"flagLinkSegment\":\"0/00/Flag_of_Palestine.svg\",\"phoneNumberRegex\":\"^5[69]\\\\d{7}$\"},{\"name\":\"Qatar\",\"prefix\":\"+974\",\"flagLinkSegment\":\"6/65/Flag_of_Qatar.svg\",\"phoneNumberRegex\":\"^3[0-9]\\\\d{7}$\"},{\"name\":\"KSA\",\"prefix\":\"+966\",\"flagLinkSegment\":\"0/0d/Flag_of_Saudi_Arabia.svg\",\"phoneNumberRegex\":\"^5[0-9]\\\\d{7}$\"},{\"name\":\"Somalia\",\"prefix\":\"+252\",\"flagLinkSegment\":\"a/a0/Flag_of_Somalia.svg\",\"phoneNumberRegex\":\"^7[0-9]\\\\d{7}$\"},{\"name\":\"Sudan\",\"prefix\":\"+249\",\"flagLinkSegment\":\"0/01/Flag_of_Sudan.svg\",\"phoneNumberRegex\":\"^9[0-9]\\\\d{7}$\"},{\"name\":\"Syria\",\"prefix\":\"+963\",\"flagLinkSegment\":\"5/53/Flag_of_Syria.svg\",\"phoneNumberRegex\":\"^9[1-9]\\\\d{7}$\"},{\"name\":\"Tunisia\",\"prefix\":\"+216\",\"flagLinkSegment\":\"c/ce/Flag_of_Tunisia.svg\",\"phoneNumberRegex\":\"^9[0-9]\\\\d{7}$\"},{\"name\":\"UAE\",\"prefix\":\"+971\",\"flagLinkSegment\":\"c/cb/Flag_of_the_United_Arab_Emirates.svg\",\"phoneNumberRegex\":\"^5[0-9]\\\\d{7}$\"},{\"name\":\"Yemen\",\"prefix\":\"+967\",\"flagLinkSegment\":\"8/89/Flag_of_Yemen.svg\",\"phoneNumberRegex\":\"^7[0-9]\\\\d{7}$\"}]";

            Countries = JsonSerializer.Deserialize<List<CountryDetailsDto>>(stringifiedCountriesDetails, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var ksa = Countries.FirstOrDefault(c => c.Prefix == "+966");
            if (ksa is not null)
            {
                SelectedCountryPrefix = ksa.Prefix;
                CurrentPhoneNumberRegex = ksa.PhoneNumberRegex;
            }
        }

        private void GetSelectedCountryPrefix(string prefix)
        {
            SelectedCountryPrefix = prefix;
            CurrentPhoneNumberRegex = Countries.FirstOrDefault(c => c.Prefix == SelectedCountryPrefix)?.PhoneNumberRegex ?? string.Empty;
        }

        private void ValidatePhoneNumberFormat(string value)
        {
            IsPhoneNumberRegexErrorShown = !Regex.IsMatch(value, CurrentPhoneNumberRegex);
            registerRequest.PhoneNumber = value;
        }

        private void TogglePasswordVisibility()
        {
            if (passwordInputType == InputType.Password)
            {
                passwordInputType = InputType.Text;
                passwordIcon = Icons.Material.Filled.Visibility;
            }
            else
            {
                passwordInputType = InputType.Password;
                passwordIcon = Icons.Material.Filled.VisibilityOff;
            }
        }

        private void ToggleConfirmingPasswordVisibility()
        {
            if (confirmingPasswordInputType == InputType.Password)
            {
                confirmingPasswordInputType = InputType.Text;
                confirmingPasswordIcon = Icons.Material.Filled.Visibility;
            }
            else
            {
                confirmingPasswordInputType = InputType.Password;
                confirmingPasswordIcon = Icons.Material.Filled.VisibilityOff;
            }
        }

        private async Task OpenTermsAndConditionsDialogAsync()
        {
            var dialog = await DialogService.ShowAsync<TermsAndConditionsDialog>(Resource.TermsAndConditions);
            var result = await dialog.Result;
        }
    }
}