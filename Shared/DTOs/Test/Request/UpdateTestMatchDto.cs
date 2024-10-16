using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Test.Request
{
    public class UpdateTestMatchDto
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        public TimeSpan? EventTime { get; set; }

        [Required]
        public long StadiumId { get; set; }

        [Required]
        public long ChampionId { get; set; }
    }

}
