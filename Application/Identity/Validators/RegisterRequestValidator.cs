using Domain.Entities.UserEntities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Shared.Common.General;
using Shared.DTOs.Identity.Register;
using Shared.ResourceFiles;

namespace Application.Identity.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterRequestValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage(Resource.UserName_Validation);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(Resource.Email_Required_Validation)
                .Matches(RegexTemplates.Email).WithMessage(Resource.Email_Format_Validation);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(Resource.Password_Validation)
                .Matches(RegexTemplates.Password).WithMessage(Resource.Password_Format_Validation);

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage(Resource.Confirming_Password_Validation)
                .Equal(x => x.Password).WithMessage(Resource.Passwords_NotMatching);

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage(Resource.PhoneNumber_Validation)
                .Must(BeUniquePhoneNumber).WithMessage(Resource.PhoneNumber_Unique_Validation);
        }

        // Custom validation for checking if the phone number is unique
        private bool BeUniquePhoneNumber(string phoneNumber)
        {
            return _userManager.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber) is null;
        }
    }

}
