using Application.Interfaces.Persistence;
using AutoMapper;
using Domain.Entities.UserEntities;
using LanguageExt;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Common;
using Shared.Common.General;
using Shared.DTOs.Authorization.Request;
using Shared.DTOs.Authorization.Response;
using Shared.DTOs.Identity.UserDTOs;
using Shared.Exceptions;
using Shared.ResourceFiles;

namespace Application.Permissions.Service
{
    public class AuthorizationService(RoleManager<ApplicationRole> roleManager,
                                      UserManager<ApplicationUser> userManager,
                                      IMapper mapper,
                                      ITransactionRepository transactionRepository,
                                      IAuthorizationRepository authorizationRepository) : IAuthorizationService
    {
        #region Fields
        private readonly IMapper _mapper = mapper;
        private readonly ITransactionRepository _transactionRepository = transactionRepository;
        private readonly IAuthorizationRepository _authorizationRepository = authorizationRepository;
        private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        #endregion

        #region Role Operations
        public async Task<BaseResponse<Empty>> AddRoleAsync(AddRoleDto addRoleDto)
        {

            var identityRole = new ApplicationRole { Name = addRoleDto.RoleName, Description = addRoleDto.RoleDescription };

            var result = await _roleManager.CreateAsync(identityRole);

            if (!result.Succeeded)
            {
                var addRoleFailedExp = new BaseException(
                    result.Errors.Select(e => new ErrorInfo
                    {
                        Title = Resource.CreateRoleFailed,
                        Message = e.Description
                    }).ToList());

                return addRoleFailedExp;
            }
            return new();
        }


        public async Task<BaseResponse<Empty>> DeleteRoleAsync(long roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role?.Name is null)
            {
                var notFoundExp = new NotFoundException(
                [
                    new(){ Title = Resource.RoleDeletionFailed, Message = Resource.NotFound }
                ]);
                return notFoundExp;
            }

            //var users = await _userManager.GetUsersInRoleAsync(role.Name);

            //if (users != null && users.Any())
            //{
            //    var roleInUseExp = new BaseException(
            //    [
            //        new(){ Title = Resource.RoleDeletionFailed, Message = Resource.RoleAssignedToUsers }
            //    ]);
            //    return new(roleInUseExp);
            //}

            // Delete the role
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
                return new(Empty.Default);

            var deleteFailedExp = new BaseException(
                result.Errors.Select(e => new ErrorInfo
                {
                    Title = Resource.RoleDeletionFailed,
                    Message = e.Description
                }).ToList());
            return deleteFailedExp;
        }

        public async Task<BaseResponse<Empty>> EditRoleAsync(EditRoleDto editRoleDto)
        {
            var role = await _roleManager.FindByIdAsync(editRoleDto.RoleId);

            if (role == null)
            {
                var roleNotFoundExp = new NotFoundException(
                [
                    new(){ Title = Resource.EditRoleFailed, Message = string.Format(Resource.NotFound, editRoleDto.RoleId) }
                ]);
                return roleNotFoundExp;
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
                return updateRoleFailedExp;
            }

            return new();

        }

        public async Task<BaseResponse<RoleDto>> GetRoleById(long id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                var roleNotFoundExp = new NotFoundException(
                [
                    new(){ Title = Resource.GetRoleFailed, Message = string.Format(Resource.NotFound) }
                ]);
                return roleNotFoundExp;
            }

            var roleDto = _mapper.Map<RoleDto>(role);
            return roleDto;
        }

        public async Task<BaseResponse<List<RoleDto>>> GetRolesList(CancellationToken cancellationToken)
        {
            var roles = await _roleManager.Roles.ToListAsync(cancellationToken);
            var rolesDto = _mapper.Map<List<RoleDto>>(roles);
            return rolesDto;
        }

        public async Task<bool> IsRoleExist(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        public async Task<bool> IsRoleExistById(long roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            return role != null;
        }

        #endregion

        #region User-Role Assigning Operations

        public async Task<BaseResponse<Empty>> AssignUserToRolesAsync(AssignUserToRolesDto assignUserToRolesDto)
        {

            // Find the user by ID
            var user = await _userManager.FindByIdAsync(assignUserToRolesDto.UserId.ToString());
            if (user == null)
            {
                var userNotFoundExp = new NotFoundException(
                [

            new() { Title = Resource.AssignRoleFailed, Message = string.Format(Resource.NotFound, assignUserToRolesDto.UserId) }
                ]);
                return userNotFoundExp;
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
                    return removeRoleFailedExp;
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
                    return addRoleFailedExp;
                }
            }

            // Success if roles were added or removed correctly
            return Empty.Default;

        }

        public async Task<BaseResponse<Empty>> AssignUsersToRoleAsync(AssignUsersToRoleDto assignUsersToRoleDto, CancellationToken cancellationToken = default)
        {

            // Validate input
            if (assignUsersToRoleDto is null || assignUsersToRoleDto.RoleId == 0 || !assignUsersToRoleDto.UserIds.Any())
            {
                var validationExp = new BaseException(
                    [ new (){ Title = Resource.AssignRoleFailed, Message = "Invalid input data." }
                    ]);
                return validationExp;
            }

            return await _authorizationRepository.AssignUsersToRoleAsync(assignUsersToRoleDto, cancellationToken);

        }

        public async Task<BaseResponse<Empty>> RemoveUsersFromRoleAsync(RemoveUsersFromRoleDto removeUsersFromRoleDto, CancellationToken cancellationToken = default)
        {

            if (removeUsersFromRoleDto == null || removeUsersFromRoleDto.RoleId == 0 || !removeUsersFromRoleDto.UserIds.Any())
            {
                var validationExp = new BaseException(
                    [
                    new () { Title = Resource.RemoveRoleFailed, Message = "Invalid input data." }
                    ]);
                return validationExp;
            }

            return await _authorizationRepository.UnassignUsersFromRoleAsync(removeUsersFromRoleDto, cancellationToken);

        }

        public async Task<BaseResponse<RoleWithUsersDto>> GetRoleWithUsersAsync(long roleId, PaginationSearchModel paginationSearchModel, CancellationToken cancellationToken)
        {

            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role?.Name is null)
            {
                var roleNotFoundExp = new NotFoundException(
                [
                    new() { Title = Resource.GetRoleFailed, Message = string.Format(Resource.NotFound, roleId) }
                ]);
                return roleNotFoundExp;
            }

            // Use repository to fetch users and apply pagination
            var usersInRoleQuery = _authorizationRepository.GetUsersInRole(roleId);
            var filterdUsers = SearchFilterPagination.Filter(usersInRoleQuery, paginationSearchModel);
            var usersCount = await filterdUsers.CountAsync();
            var paginatedUserList = await SearchFilterPagination.PaginateData(filterdUsers, paginationSearchModel);

            // Map the paginated users to DTO
            var roleAppUsers = paginatedUserList.Select(_mapper.Map<ApplicationUserDTO>).ToList();

            var roleWithUsersDto = new RoleWithUsersDto
            {
                RoleId = role.Id,
                RoleName = role.Name,
                Description = role.Description,
                AssignedUsers = new(roleAppUsers, usersCount, paginationSearchModel.PageIndex, paginationSearchModel.PageSize),
            };

            return new(roleWithUsersDto);

        }

        public async Task<BaseResponse<UserWithRolesDto>> GetUserWithRolesAsync(long userId)
        {

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
            {
                var userNotFoundExp = new NotFoundException(
                [
                    new() { Title = Resource.NotFound, Message = Resource.NotFoundInDB_Message }
                ]);
                return userNotFoundExp;
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

        public async Task<BaseResponse<PaginatedList<UserWithRolesDto>>> GetUsersWithRolesAsync(PaginationSearchModel paginationSearchModel, CancellationToken cancellationToken)
        {

            var filteredUsersQuery = SearchFilterPagination.Filter(_userManager.Users, paginationSearchModel);

            var totalItems = await filteredUsersQuery.CountAsync(cancellationToken);

            var items = _authorizationRepository.GetUsersWithRolesAsync(filteredUsersQuery, cancellationToken);

            var paginatedList = PaginatedList<UserWithRolesDto>.Create(items, totalItems, paginationSearchModel.PageIndex, paginationSearchModel.PageSize);
            return paginatedList;

        }
        #endregion
    }
}