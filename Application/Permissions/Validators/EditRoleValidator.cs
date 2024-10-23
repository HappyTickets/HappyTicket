//using Domain.Entities.UserEntities.AuthEntities;
//using FluentValidation;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.Localization;
//using Shared.DTOs.Authorization.Request;
//using Shared.ResourceFiles;

//namespace Application.Permissions.Validators
//{
//    public class EditRoleValidator : AbstractValidator<EditRoleDto>
//    {
//        #region Fields
//        private readonly IStringLocalizer<Resource> _stringLocalizer;
//        private readonly RoleManager<Role> _roleManager;
//        #endregion
//        #region Constructors

//        #endregion
//        public EditRoleValidator(IStringLocalizer<Resource> stringLocalizer
//                                 , RoleManager<Role> roleManager)
//        {
//            _stringLocalizer = stringLocalizer;
//            _roleManager = roleManager;
//            ApplyValidationsRules();
//        }

//        #region Actions
//        public void ApplyValidationsRules()
//        {
//            RuleFor(x => x.RoleId)
//                .NotEmpty()
//                .NotNull()
//                .WithMessage(_stringLocalizer[Resource.RequiredFields]);
//        }


//        public void ApplyCustomValidationsRules()
//        {
//            RuleFor(x => x.RoleName)
//             .NotEmpty().WithMessage(_stringLocalizer[Resource.RequiredFields])
//             .When(x => !string.IsNullOrEmpty(x.RoleName))
//             .MustAsync(async (dto, roleName, cancellationToken) =>
//             {
//                 var role = await _roleManager.FindByNameAsync(roleName);
//                 return role == null || role.Id == dto.RoleId;
//             })
//             .WithMessage(_stringLocalizer[Resource.RoleAlreadyExists])
//             .When(x => !string.IsNullOrEmpty(x.RoleName));

//        }



//        #endregion
//    }
//}
