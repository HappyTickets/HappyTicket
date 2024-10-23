using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.ChampionDtos
{
    public class UpdateChampionshipDto
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Logo { get; set; }
    }
}
