using Domain.Entities.UserEntities;

namespace Domain.Entities
{
    public class Team: BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Logo { get; set; }
        public virtual ICollection<Ticket>? Tickets { get; set; }
        public virtual ICollection<UserFavoriteTeam>? UserFavoriteTeams { get; set; }
    }
}
