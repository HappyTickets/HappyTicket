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

                if (editRoleDto.RoleName != null)
                {
                    role.Name = editRoleDto.RoleName;
                }
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

        public async Task<Result<Unit>> AssignUserToRolesAsync(AssignUserToRolesDto assignUserToRolesDto)
        {
            try
            {
                // Find the user by ID
                var user = await _userManager.FindByIdAsync(assignUserToRolesDto.UserId);
                if (user == null)
                {
                    var userNotFoundExp = new NotFoundException(
                    [

                new() { Title = Resource.AssignRoleFailed, Message = string.Format(Resource.NotFound, assignUserToRolesDto.UserId) }
                    ]);
                    return new(userNotFoundExp);
                }

                // Fetch current roles of the user
                var currentRoles = await _userManager.GetRolesAsync(user);

                var rolesToRemove = currentRoles.Except(assignUserToRolesDto.Roles).ToList();
                var rolesToAdd = assignUserToRolesDto.Roles.Except(currentRoles).ToList();

                // Remove roles if necessary
                if (rolesToRemove.Any())
                {
                    var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                    if (!removeResult.Succeeded)
                    {
                        var errorMessages = string.Join(", ", removeResult.Errors.Select(e => e.Description));
                        var removeRoleFailedExp = new BaseException(
                        [
                    new() { Title = Resource.UserRemovedFromRoleFailed, Message = errorMessages }
                        ]);
                        return new(removeRoleFailedExp);
                    }
                }

                // Assign new roles if any
                if (rolesToAdd.Any())
                {
                    var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
                    if (!addResult.Succeeded)
                    {
                        var errorMessages = string.Join(", ", addResult.Errors.Select(e => e.Description));
                        var addRoleFailedExp = new BaseException(
                        [

                    new() { Title = Resource.AssignRoleFailed, Message = errorMessages }
                        ]);
                        return new(addRoleFailedExp);
                    }
                }

                // Success if roles were added or removed correctly
                return new(Unit.Default);
            }
            catch (Exception ex)
            {
                return new(ex);
            }
        }

        public async Task<Result<Unit>> AssignUsersToRoleAsync(AssignUsersToRoleDto assignUsersToRoleDto)
        {
            var res = new Result<Unit>();

            try
            {
                // Validate input
                if (assignUsersToRoleDto == null || string.IsNullOrEmpty(assignUsersToRoleDto.Role) || !assignUsersToRoleDto.UserIds.Any())
                {
                    var validationExp = new BaseException(
                        [
                            new() { Title = Resource.AssignRoleFailed, Message = "Invalid input data." }
                        ]);
                    return new(validationExp);
                }

                // Find the role by ID (assuming Role is an ID; if it's a name, use FindByNameAsync)
                var role = await _roleManager.FindByIdAsync(assignUsersToRoleDto.Role);
                if (role == null)
                {
                    var roleNotFoundExp = new NotFoundException(
                        [
                            new() { Title = Resource.AssignRoleFailed, Message = string.Format(Resource.NotFound, assignUsersToRoleDto.Role) }
                        ]);
                    return new(roleNotFoundExp);
                }

                // Retrieve all users in one DB call
                var users = await _userManager.Users
                    .Where(u => assignUsersToRoleDto.UserIds.Contains(u.Id))
                    .ToListAsync();

                // Create a list to hold any errors encountered
                var errors = new List<string>();

                // Iterate through the users and assign the role
                foreach (var user in users)
                {
                    try
                    {
                        var isInRole = await _userManager.IsInRoleAsync(user, role.Name);
                        if (!isInRole)
                        {
                            var result = await _userManager.AddToRoleAsync(user, role.Name);
                            if (!result.Succeeded)
                            {
                                errors.AddRange(result.Errors.Select(e => e.Description));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        errors.Add(ex.Message);
                    }
                }

                // If there are any errors, return them in the response
                if (errors.Count > 0)
                {
                    var assignRoleFailedExp = new BaseException(
                        [
                            new() { Title = Resource.AssignRoleFailed, Message = "See Details", Details = errors }
                        ]);

                    return new(assignRoleFailedExp);
                }

                return new(Unit.Default); // Return success if no errors occurred
            }
            catch (Exception ex)
            {
                return new(ex); // Return a general error if something unexpected occurs
            }
        }


        public async Task<Result<Unit>> RemoveUsersFromRoleAsync(RemoveUsersFromRoleDto removeUserFromRoleDto)
        {
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
                var users = await _userManager.Users
                    .Where(u => removeUserFromRoleDto.UserIds.Contains(u.Id))
                    .ToListAsync();
                var errors = new List<string>();
                foreach (var user in users)
                {

                    if (await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        var result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                        if (!result.Succeeded)
                        {
                            errors.Add(result.Errors.First().Description);
                        }
                    }
                }
                if (errors.Any())
                {
                    var removeUsersFromRoleFailedExp = new BaseException(
                    [
                                new(){ Title = Resource.RemoveRoleFailed,Message= "",Details=errors }
                            ]);

                    return new(removeUsersFromRoleFailedExp);
                }
                return new(Unit.Default);
            }
            catch (Exception ex)
            {
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

        public async Task<Result<List<UserWithRolesDto>>> GetUsersWithRolesAsync()
        {
            try
            {
                var users = _userManager.Users.ToList();

                var usersWithRoles = new List<UserWithRolesDto>();

                foreach (var user in users)
                {
                    var rolesNames = await _userManager.GetRolesAsync(user);

                    var userWithRolesDto = new UserWithRolesDto
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        AssignedRoles = rolesNames.ToList()
                    };
                    usersWithRoles.Add(userWithRolesDto);
                }

                return new(usersWithRoles);
            }
            catch (Exception ex)
            {
                return new(ex);
            }
        }



        #endregion
    }
}
