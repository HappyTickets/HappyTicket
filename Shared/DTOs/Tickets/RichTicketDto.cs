namespace Shared.DTOs.Tickets
{
    public class RichTicketDto
    {
        public string TicketId { get; set; }
        public decimal Price { get; set; }
        public string Notes { get; set; }
        public string Location { get; set; }
        public string Class { get; set; }
        public string TicketStatus { get; set; }
        public MatchTeamDto SelectedTeam { get; set; }

        public TeamDto HomeTeam { get; set; }
        public TeamDto AwayTeam { get; set; }
        public StadiumDto Stadium { get; set; }
        public ChampionDto Champion { get; set; }


        public class TeamDto
        {
            public long TeamId { get; set; }
            public string TeamName { get; set; }
            public List<SponsorDto> TeamSponsors { get; set; }
        }

        public class SponsorDto
        {
            public long SponsorId { get; set; }
            public string Name { get; set; }
            public string Logo { get; set; } // Additional properties can be added
        }

        public class MatchTeamDto
        {
            public long Id { get; set; }
            public string MatchTeamName { get; set; }
        }

        public class StadiumDto
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public string Location { get; set; }
        }

        public class ChampionDto
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public List<SponsorDto> ChampionSponsors { get; set; }
        }
    }
}
