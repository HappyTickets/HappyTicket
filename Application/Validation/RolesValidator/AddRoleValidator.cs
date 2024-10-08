using Domain.Entities.UserEntities.AuthEntities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Shared.DTOs.Authorization.Request;
using Shared.ResourceFiles;

namespace Application.Validation.RolesValidator
{
    public class AddRoleValidator : AbstractValidator<AddRoleDto>
    {
        #region Fields
        private readonly IStringLocalizer<Resource> _stringLocalizer;
        private readonly RoleManager<Role> _roleManager;
        #endregion
        #region Constructors

        #endregion
        public AddRoleValidator(IStringLocalizer<Resource> stringLocalizer,
                                 RoleManager<Role> roleManager)
        {
            _stringLocalizer = stringLocalizer;
            _roleManager = roleManager;
            ApplyValidationsRules();
        }

        #region Actions
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.RoleName)
                 .NotEmpty().WithMessage(_stringLocalizer[Resource.RequiredFields])
                 .NotNull().WithMessage(_stringLocalizer[Resource.RequiredFields]);


        }


        public void ApplyCustomValidationsRules()
        {
            RuleFor(x => x.RoleName)
                .MustAsync(async (roleName, cancellationToken) => !await _roleManager.RoleExistsAsync(roleName))
                .WithMessage(_stringLocalizer[Resource.RoleAlreadyExists]);
        }


        #endregion
    }
}
