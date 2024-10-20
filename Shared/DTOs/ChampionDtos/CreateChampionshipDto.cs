using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Champion
{
    public class CreateChampionshipDto
    {
        [Required]
        public string Name { get; set; }
        public string? Logo { get; set; }
    }
}
