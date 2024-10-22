using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Test.Request
{
    public class CreateTestMatchDto

    {
        [Required]
        public DateTime EventDate { get; set; }

        public TimeSpan? EventTime { get; set; }

        [Required]
        public long StadiumId { get; set; }

        [Required]
        public long ChampionId { get; set; }
    }

}
