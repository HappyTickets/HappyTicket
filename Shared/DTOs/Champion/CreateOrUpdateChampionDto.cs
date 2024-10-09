namespace Shared.DTOs.Champion
{
    public class CreateOrUpdateChampionDto
    {
        public string Name { get; set; }
        public string Logo { get; set; }

        public IEnumerable<Guid>? SponsorsIds { get; set; }
    }
}
