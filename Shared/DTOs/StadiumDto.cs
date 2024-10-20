namespace Shared.DTOs
{
    public class StadiumDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? Photo { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public string? LocationUrl { get; set; }
        public List<BlockDto> Blocks { get; set; } = new List<BlockDto>();
    }
}
