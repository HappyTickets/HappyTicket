namespace Shared.DTOs.MatchDtos
{
    public class MatchCommandDto
    {
        public Guid TeamAId { get; set; }
        public Guid TeamBId { get; set; }
        public Guid StadiumId { get; set; }
        public DateTime? EventDate { get; set; }
        public TimeSpan? EventTime { get; set; }
        public int MaxPerUser { get; set; } = 1;
    }
}
