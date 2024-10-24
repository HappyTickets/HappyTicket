namespace Shared.DTOs.Sponsors
{
    public class CreateOrUpdateSponsorDto
    {
        public string Name { get; set; }
        public string? Logo { get; set; }
        public bool IsHappySponsor { get; set; }
    }
}
