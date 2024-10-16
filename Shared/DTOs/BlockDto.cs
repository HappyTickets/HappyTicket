namespace Shared.DTOs
{
    public class BlockDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<SeatDto> Seats { get; set; } = new List<SeatDto>();
    }


}
