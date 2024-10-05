using Application.Interfaces.IIdentityServices;
using Application.Interfaces.Persistence;
using AutoMapper;
using Domain.Entities.UserEntities;
using Domain.Entities.UserEntities.AuthEntities;
using FluentValidation;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Shared.DTOs.Authorization.Request;
using Shared.DTOs.Authorization.Response;
using Shared.DTOs.Identity.UserDTOs;
using Shared.Exceptions;
using Shared.ResourceFiles;

namespace Application.Implementations.IdentityServices
{
    public class AuthorizationService(RoleManager<Role> roleManager,
                                      UserManager<ApplicationUser> userManager,
                                      IStringLocalizer<Resource> localizer,
                                      IMapper mapper,
                                      ITransactionRepository transactionRepository,
                                      IValidator<AddRoleDto> addValidator,
                                      IValidator<EditRoleDto> editValidator) : IAuthorizationService
    {
        #region Fields
        private readonly IMapper _mapper = mapper;
        private readonly ITransactionRepository _transactionRepository = transactionRepository;
        private readonly IValidator<AddRoleDto> _addValidator = addValidator;
        private readonly IValidator<EditRoleDto> _editValidator = editValidator;
        private readonly RoleManager<Role> _roleManager = roleManager;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IStringLocalizer<Resource> _localizer = localizer;
        #endregion

        #region Role Operations
        public async Task<Result<Unit>> AddRoleAsync(AddRoleDto addRoleDto)
        {
            try
            {
                var validationResult = _addValidator.Validate(addRoleDto);
                if (!validationResult.IsValid)
                {
                    return new(new EntityValidationException(validationResult.Errors));
                }

                var identityRole = new Role { Name = addRoleDto.RoleName, Description = addRoleDto.RoleDescription };

                var result = await _roleManager.CreateAsync(identityRole);

                if (!result.Succeeded)
                {
                    var addRoleFailedExp = new BaseException(
                        result.Errors.Select(e => new ErrorInfo
                        {
                            Title = Resource.CreateRoleFailed,
                            Message = e.Description
                        }).ToList());

                    return new(addRoleFailedExp);
                }
                return new(Unit.Default);
            }
            catch (Exception ex)
            {
                return new(ex);
            }
        }


        public async Task<Result<Unit>> DeleteRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role?.Name is null)
            {
                var notFoundExp = new NotFoundException(
                [
                    new(){ Title = Resource.RoleDeletionFailed, Message = Resource.NotFound }
                ]);
                return new(notFoundExp);
            }

            // Check if users are assigned to the role
            var users = await _userManager.GetUsersInRoleAsync(role.Name);

            if (users != null && users.Any())
            {
                var roleInUseExp = new BaseException(
                [
                    new(){ Title = Resource.RoleDeletionFailed, Message = Resource.RoleAssignedToUsers }
                ]);
                return new(roleInUseExp);
            }

            // Delete the role
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
                return new(Unit.Default);

            var deleteFailedExp = new BaseException(
                result.Errors.Select(e => new ErrorInfo
                {
                    Title = Resource.RoleDeletionFailed,
                    Message = e.Description
                }).ToList());
            return new(deleteFailedExp);
        }

        public async Task<Result<Unit>> EditRoleAsync(EditRoleDto editRoleDto)
        {
            try
            {
                var validationResult = _editValidator.Validate(editRoleDto);
                if (!validationResult.IsValid)
                {
                    return new(new EntityValidationException(validationResult.Errors));
                }

                var role = await _roleManager.FindByIdAsync(editRoleDto.RoleId);

                if (role == null)
                {
                    var roleNotFoundExp = new NotFoundException(
                    [
                        new(){ Title = Resource.EditRoleFailed, Message = string.Format(Resource.NotFound, editRoleDto.RoleId) }
                    ]);
                    return new(roleNotFoundExp);
                }

                // Proceed with role update logic
                role.Name = editRoleDto.RoleName;
                role.Description = editRoleDto.RoleDescription;

                var result = await _roleManager.UpdateAsync(role);

                if (!result.Succeeded)
                {
                    var updateRoleFailedExp = new BaseException(
                    [
                        new(){ Title = Resource.EditRoleFailed, Message = string.Join(", ", result.Errors.Select(e => e.Description)) }
                    ]);
                    return new(updateRoleFailedExp);
                }

                return new(Unit.Default);
            }
            catch (Exception ex)
            {
                return new(ex);
            }
        }

        public async Task<Result<RoleDto>> GetRoleById(string id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                {
                    var roleNotFoundExp = new NotFoundException(
                    [
                        new(){ Title = Resource.GetRoleFailed, Message = string.Format(Resource.NotFound) }
                    ]);
                    return new Result<RoleDto>(roleNotFoundExp);
                }

                var roleDto = _mapper.Map<RoleDto>(role);
                return new Result<RoleDto>(roleDto);
            }
            catch (Exception ex)
            {
                return new Result<RoleDto>(ex);
            }
        }

        public async Task<Result<List<RoleDto>>> GetRolesList()
        {
            try
            {
                var roles = await _roleManager.Roles.ToListAsync();
                var rolesDto = _mapper.Map<List<RoleDto>>(roles);
                return new(rolesDto);
            }
            catch (Exception ex)
            {
                return new(ex);
            }
        }

        public async Task<bool> IsRoleExist(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        public async Task<bool> IsRoleExistById(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            return role != null;
        }

        #endregion

        #region User-Role Assigning Operations

        // Assign a single user to a specific role
        public async Task<Result<Unit>> AssignUserToRoleAsync(AssignUserToRoleDto assignUserToRoleDto)
        {
            await _transactionRepository.BeginTransactionAsync();
            try
            {
                // Check if the role exists
                var role = await _roleManager.FindByIdAsync(assignUserToRoleDto.RoleId);
                if (role is null)
                {
                    var roleNotFoundExp = new NotFoundException(
                    [
                        new(){ Title = Resource.AssignRoleFailed, Message = string.Format(Resource.NotFound, assignUserToRoleDto.RoleId) }
                    ]);
                    return new(roleNotFoundExp);
                }

                // Find the user by ID
                var user = await _userManager.FindByIdAsync(assignUserToRoleDto.UserId);
                if (user is null)
                {
                    var userNotFoundExp = new NotFoundException(
                    [
                        new(){ Title = Resource.AssignRoleFailed, Message = string.Format(Resource.NotFound, assignUserToRoleDto.UserId) }
                    ]);
                    return new(userNotFoundExp);
                }

                // Check if the user is already in the role
                if (!(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    var result = await _userManager.AddToRoleAsync(user, role.Name);
                    if (!result.Succeeded)
                    {
                        var addUserToRoleFailedExp = new BaseException(
                        [
                            new(){ Title = Resource.AssignRoleFailed, Message = string.Join(", ", result.Errors.Select(e => e.Description)) }
                        ]);
                        return new(addUserToRoleFailedExp);
                    }
                }

                await _transactionRepository.CommitTransactionAsync();
                return new(Unit.Default);
            }
            catch (Exception ex)
            {
                await _transactionRepository.RollbackTransactionAsync(); // Rollback in case of error
                return new(ex);
            }
        }


        public async Task<Result<Unit>> RemoveUserFromRoleAsync(RemoveUserFromRoleDto removeUserFromRoleDto)
        {
            await _transactionRepository.BeginTransactionAsync();
            try
            {
                // Check if the role exists
                var role = await _roleManager.FindByIdAsync(removeUserFromRoleDto.RoleId);
                if (role == null)
                {
                    var roleNotFoundExp = new NotFoundException(
                    [
                        new(){ Title = Resource.RemoveRoleFailed, Message = string.Format(Resource.NotFound, removeUserFromRoleDto.RoleId) }
                    ]);
                    return new(roleNotFoundExp);
                }

                // Find the user by ID
                var user = await _userManager.FindByIdAsync(removeUserFromRoleDto.UserId);
                if (user is null)
                {
                    var userNotFoundExp = new NotFoundException(
                    [
                        new(){ Title = Resource.RemoveRoleFailed, Message = string.Format(Resource.NotFound, removeUserFromRoleDto.UserId) }
                    ]);
                    return new(userNotFoundExp);
                }

                // Check if the user is in the role
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    var result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                    if (!result.Succeeded)
                    {
                        var removeUserFromRoleFailedExp = new BaseException(
                        [
                            new(){ Title = Resource.RemoveRoleFailed, Message = string.Join(", ", result.Errors.Select(e => e.Description)) }
                        ]);
                        return new(removeUserFromRoleFailedExp);
                    }
                }

                await _transactionRepository.CommitTransactionAsync(); // Commit the transaction after successful role removal
                return new(Unit.Default);
            }
            catch (Exception ex)
            {
                await _transactionRepository.RollbackTransactionAsync(); // Rollback in case of error
                return new(ex);
            }
        }

        public async Task<Result<RoleWithUsersDto>> GetRoleWithUsersAsync(string roleId)
        {
            try
            {
                // Find the role by ID
                var role = await _roleManager.FindByIdAsync(roleId);
                if (role?.Name is null)
                {
                    var roleNotFoundExp = new NotFoundException(
                    [
                        new() { Title = Resource.GetRoleFailed, Message = string.Format(Resource.NotFound, roleId) }
                    ]);
                    return new(roleNotFoundExp);
                }

                // Get the users assigned to this role
                var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);

                var roleAppUsers = _mapper.Map<List<ApplicationUserDTO>>(usersInRole);

                // Map the role and assigned users to the DTO
                var roleWithUsersDto = new RoleWithUsersDto
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    Description = role.Description,
                    AssignedUsers = roleAppUsers
                };

                return new Result<RoleWithUsersDto>(roleWithUsersDto);
            }
            catch (Exception ex)
            {
                return new Result<RoleWithUsersDto>(ex);
            }
        }

        public async Task<Result<UserWithRolesDto>> GetUserWithRolesAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user is null)
                {
                    var userNotFoundExp = new NotFoundException(
                    [
                        new() { Title = Resource.NotFound, Message = Resource.NotFoundInDB_Message }
                    ]);
                    return new(userNotFoundExp);
                }

                // Get the roles assigned to this user
                var rolesNames = await _userManager.GetRolesAsync(user);

                // Map the user and assigned roles to the DTO
                var userWithRolesDto = new UserWithRolesDto
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    AssignedRoles = rolesNames.ToList()
                };

                return new(userWithRolesDto);
            }
            catch (Exception ex)
            {
                return new(ex);
            }
        }




        #endregion
    }
}
