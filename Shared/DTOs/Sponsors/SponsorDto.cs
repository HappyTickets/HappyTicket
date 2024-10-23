namespace Shared.DTOs.Sponsors
{
    public class SponsorDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? Logo { get; set; }
        public bool? IsHappySponsor { get; set; }
    }
}
