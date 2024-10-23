namespace Shared.DTOs.MatchDtos
{
    public class GetAllMatchesDto
    {

        public string StadiumName { get; set; } = string.Empty;
        public string ChampionName { get; set; } = string.Empty;
        public DateTime? EventDate { get; set; }
        public TimeSpan? EventTime { get; set; }
        public string TeamAName { get; set; } = string.Empty;
        public string TeamALogo { get; set; } = string.Empty;
        public string TeamBName { get; set; } = string.Empty;
        public string TeamBLogo { get; set; } = string.Empty;




    }
}
