namespace Shared.DTOs.Test.Response
{
    public class GetMatchDto
    {
        public long Id { get; set; }
        public DateTime? EventDate { get; set; }
        public TimeSpan? EventTime { get; set; }
        public string StadiumName { get; set; } = string.Empty;
        public string ChampionName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

}
