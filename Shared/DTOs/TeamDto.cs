﻿namespace Shared.DTOs
{
    public class TeamDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Logo { get; set; }
        public ICollection<Guid>? TicketIds { get; set; }
        public ICollection<Guid>? UserFavoriteTeamIds { get; set; }
    }
}
