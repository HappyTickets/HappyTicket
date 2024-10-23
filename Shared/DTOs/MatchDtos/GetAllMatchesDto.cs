namespace Shared.DTOs.MatchDtos
{
    public class GetAllMatchesDto : BaseMatch
    {

        public string StadiumName { get; set; } = string.Empty;
        public string ChampionName { get; set; } = string.Empty;
        public bool? HasTickets { get; set; } = false;



    }
}
