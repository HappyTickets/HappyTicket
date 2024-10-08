using FluentValidation;
using Shared.DTOs.Champion;

namespace Application.Validation
{
    public class ChampionValidator: AbstractValidator<ChampionDto>
    {
        public ChampionValidator()
        {
            
        }
    }
}
