using Application.Interfaces;
using Domain.Entities;
using Shared.DTOs.MatchDtos;

namespace Application.Implementations
{

    public interface IMatchCommandService : IBaseService<MatchO, MatchCommandDto>
    {
    }
}