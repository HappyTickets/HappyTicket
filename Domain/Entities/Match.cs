﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Match : BaseEntity
    {
        public int MaxPerUser { get; set; } = 1;
        public DateTime? EventDate { get; set; }
        public TimeSpan? EventTime { get; set; }

        public Guid TeamAId { get; set; }
        [ForeignKey(nameof(TeamAId))]
        public virtual Team? TeamA { get; set; }

        public Guid? TeamBId { get; set; }
        [ForeignKey(nameof(TeamBId))]
        public virtual Team? TeamB { get; set; }

        public Guid? ChampionId { get; set; }
        [ForeignKey(nameof(ChampionId))]
        public virtual Champion? Champion { get; set; }

        public Guid StadiumId { get; set; }
        [ForeignKey(nameof(StadiumId))]
        public virtual Stadium? Stadium { get; set; }

        public virtual ICollection<Ticket>? Tickets { get; set; }
    }
}
