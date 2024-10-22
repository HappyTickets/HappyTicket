//using Application.Interfaces.IIdentityServices;
//using Application.Interfaces.Persistence;
//using Domain.Entities.UserEntities;
//using LanguageExt.Common;
//using Shared.Common.General;
//using Shared.DTOs.Identity.UserDTOs;

//namespace Application.Implementations.IdentityServices
//{
//    public class UserService(IUserRepository<ApplicationUser> userRepository) : IUserService
//    {
//        private readonly IUserRepository<ApplicationUser> _userRepository = userRepository;

//        public async Task<Result<PaginatedList<ApplicationUserDTO>>> GetAllUsersAsync(PaginationSearchModel queryParams, CancellationToken cancellationToken)
//        {
//            try
//            {
//                // Fetch the paginated users from the repository
//                var usersResult = await _userRepository.GetAllAsync(queryParams, cancellationToken);

//                return usersResult.Match(
//                    paginatedUsers =>
//                    {
//                        // Map the list of users to a list of ApplicationUserDTOs
//                        var usersDtoList = paginatedUsers.Items.Select(user => new ApplicationUserDTO
//                        {
//                            Id = user.Id,
//                            UserName = user.UserName,
//                            Email = user.Email,
//                            PhoneNumber = user.PhoneNumber,
//                        });

//                        // Create  PaginatedList<ApplicationUserDTO> based on the mapped DTOs
//                        var paginatedDtoList = new PaginatedList<ApplicationUserDTO>(
//                            usersDtoList,
//                            paginatedUsers.TotalItems,
//                            paginatedUsers.PageIndex,
//                            queryParams.PageSize
//                        );

//                        // Return the paginated DTO list
//                        return new Result<PaginatedList<ApplicationUserDTO>>(paginatedDtoList);
//                    },
//                    ex => new Result<PaginatedList<ApplicationUserDTO>>(ex)
//                );
//            }
//            catch (Exception ex)
//            {
//                return new Result<PaginatedList<ApplicationUserDTO>>(ex);
//            }
//        }

//    }
//}