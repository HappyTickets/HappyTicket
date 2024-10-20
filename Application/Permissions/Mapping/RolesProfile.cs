using AutoMapper;
using Domain.Entities.UserEntities;
using Shared.DTOs.Authorization.Response;

namespace Application.Permissions.Mapping
{


    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<ApplicationRole, RoleDto>()
            .ForMember(r => r.RoleId, opt => opt.MapFrom(r => r.Id))
            .ForMember(r => r.RoleName, opt => opt.MapFrom(r => r.Name))
            .ForMember(r => r.RoleDescription, opt => opt.MapFrom(r => r.Description))
            .ReverseMap();
        }
    }
}
