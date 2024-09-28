namespace Shared.DTOs
{
    public class UserFavoriteTeamDto
    {
        public Guid? MatchId { get; set; }
        public string UserId { get; set; }
        public Guid? TeamId { get; set; }
    }
}
