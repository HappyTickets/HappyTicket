﻿using Application.Interfaces.Persistence;
using Domain.Entities.UserEntities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Common;
using Shared.Common.General;
using Shared.Exceptions;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Identity;

public class UserRepository<TUser> : IUserRepository<TUser> where TUser : ApplicationUser
{
    private readonly UserManager<TUser> _userManager;

    public UserRepository(UserManager<TUser> userManager)
    {
        _userManager = userManager;
    }


    #region Query

    public async Task<BaseResponse<PaginatedList<TUser>>> GetAllAsync(PaginationSearchModel queryParams, CancellationToken cancellationToken = default)
    {
        // Step 1: Apply filters 
        var filteredUsersQuery = SearchFilterPagination.Filter(_userManager.Users, queryParams);

        // Step 2: Get the total count of filtered users before pagination
        var totalItems = await filteredUsersQuery.CountAsync(cancellationToken);

        // Step 3: Apply pagination to the query
        var paginatedUsers = await SearchFilterPagination.PaginateData(filteredUsersQuery, queryParams);

        if (paginatedUsers is null || !paginatedUsers.Any())
        {
            return new NotFoundException(
                [new() { Title = "Not Found", Message = "Users cannot be found." }]);
        }

        var paginatedList = PaginatedList<TUser>.Create(paginatedUsers, totalItems, queryParams.PageIndex, queryParams.PageSize);
        return paginatedList;
    }


    public async Task<BaseResponse<TUser>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return new BadRequestException([new() { Title = "Invalid Id", Message = "The Id cannot be empty/null." }]);
        }

        TUser? user = await _userManager.FindByIdAsync(id);

        return user == null ? new NotFoundException([new() { Title = "Not Found", Message = "The user requested cannot be found." }]) : user;
    }

    public async Task<BaseResponse<TUser>> GetByUserNameAsync(string username, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return new BadRequestException([new() { Title = "Invalid Id", Message = "The Id cannot be empty/null." }]);
        }

        TUser? user = await _userManager.FindByNameAsync(username);

        return user == null ? new NotFoundException([new() { Title = "Not Found", Message = "The user requested cannot be found." }]) : user;
    }

    public async Task<BaseResponse<TUser>> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return new BadRequestException([new() { Title = "Invalid Email", Message = "The email cannot be empty/null." }]);
        }

        TUser? user = await _userManager.FindByEmailAsync(email);

        return user == null ? new NotFoundException([new() { Title = "Not Found", Message = "The user requested cannot be found." }]) : user;
    }

    public async Task<BaseResponse<IEnumerable<TUser>>> FindAsync(Expression<Func<TUser, bool>> predicate,
                                                            CancellationToken cancellationToken = default)
    {
        List<TUser> users = await _userManager.Users.Where(predicate).ToListAsync(cancellationToken);

        return users == null ? new NotFoundException([new() { Title = "Not Found", Message = "The user requested cannot be found." }]) : users;
    }

    public async Task<BaseResponse<IEnumerable<TUser>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        List<TUser> users = await _userManager.Users.ToListAsync(cancellationToken);

        return users == null ? new NotFoundException([new() { Title = "Not Found", Message = "The user requested cannot be found." }]) : users;
    }

    #endregion


    #region Command

    public async Task<BaseResponse<TUser>> CreateAsync(TUser user, string password, CancellationToken cancellationToken = default)
    {
        List<ErrorInfo> errors = [];

        TUser? emailExits = await _userManager.FindByEmailAsync(user.Email!);
        if (emailExits != null)
        {
            errors.Add(new() { Title = "Email Exists", Message = "The email provided is already being used by another user." });
        }

        TUser? usernameExits = await _userManager.FindByNameAsync(user.UserName!);
        if (usernameExits != null)
        {
            errors.Add(new() { Title = "Username Exists", Message = "The username provided is already being used by another user." });
        }

        if (errors.Any())
        {
            return new BadRequestException(errors);
        }

        IdentityResult result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            return new ServerException(
                result.Errors.Prepend(new() { Code = "Internal Server Error", Description = $"An error while trying to create the user." })
                             .Select(x => new ErrorInfo() { Title = x.Code, Message = x.Description }));
        }

        string emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        if (emailConfirmationToken != null)
        {
            _ = await _userManager.ConfirmEmailAsync(user, emailConfirmationToken);
        }

        return user;
    }
    public async Task<BaseResponse<TUser>> UpdateAsync(TUser user, CancellationToken cancellationToken = default)
    {
        List<ErrorInfo> errors = [];

        TUser? emailExits = await _userManager.FindByEmailAsync(user.Email!);
        if (emailExits != null)
        {
            errors.Add(new() { Title = "Email Exists", Message = "The email provided is already being used by another user." });
        }

        TUser? usernameExits = await _userManager.FindByNameAsync(user.UserName!);
        if (usernameExits != null)
        {
            errors.Add(new() { Title = "Username Exists", Message = "The username provided is already being used by another user." });
        }

        if (errors.Any())
        {
            return new BadRequestException(errors);
        }

        IdentityResult result = await _userManager.UpdateAsync(user);

        return result.Succeeded ? user :
    new ServerException(result.Errors.Prepend(new() { Code = "Internal Server Error", Description = $"An error while trying to update the user." })
                             .Select(x => new ErrorInfo() { Title = x.Code, Message = x.Description }));

    }

    public async Task<BaseResponse<TUser>> RecoverAsync(string Id, CancellationToken cancellationToken = default)
    {

        TUser? user = await _userManager.FindByIdAsync(Id);

        if (user == null)
        {
            return new NotFoundException([new() { Title = "Not Found", Message = "The user you're trying to restore was not found." }]);
        }

        IdentityResult result = await _userManager.DeleteAsync(user);

        return result.Succeeded
            ? user
            : new ServerException(
                result.Errors.Prepend(new() { Code = "Internal Server Error", Description = $"An error while trying to restore the user." })
                             .Select(x => new ErrorInfo() { Title = x.Code, Message = x.Description }));
    }
    public async Task<BaseResponse<TUser>> SoftDeleteAsync(string Id, CancellationToken cancellationToken = default)
    {
        TUser? user = await _userManager.FindByIdAsync(Id);

        if (user == null)
        {
            return new NotFoundException([new() { Title = "Not Found", Message = "The user you're trying to delete was not found." }]);
        }

        user.BaseEntityStatus = BaseEntityStatus.Archived;

        IdentityResult result = await _userManager.DeleteAsync(user);

        return result.Succeeded
            ? user
            : new ServerException(
                result.Errors.Prepend(new() { Code = "Internal Server Error", Description = $"An error while trying to delete the user." })
                             .Select(x => new ErrorInfo() { Title = x.Code, Message = x.Description }));
    }
    public async Task<BaseResponse<TUser>> HardDeleteAsync(string Id, CancellationToken cancellationToken = default)
    {
        TUser? user = await _userManager.FindByIdAsync(Id);

        if (user == null)
        {
            return new NotFoundException([new() { Title = "Not Found", Message = "The user you're trying to remove was not found." }]);
        }

        user.BaseEntityStatus = null;
        user.SoftDeleteCount = 0;

        IdentityResult result = await _userManager.DeleteAsync(user);

        return result.Succeeded
            ? (user)
            : new ServerException(
                result.Errors.Prepend(new() { Code = "Internal Server Error", Description = $"An error while trying to completely remove the user." })
                             .Select(x => new ErrorInfo() { Title = x.Code, Message = x.Description }));
    }



    #endregion
}