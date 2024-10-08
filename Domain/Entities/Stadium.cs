﻿namespace Domain.Entities
{
    public class Stadium : BaseEntity
    {
        public string Name { get; set; }
        public string? Photo { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public string? LocationUrl { get; set; }
        public virtual ICollection<Block>? Blocks { get; set; } = new List<Block>();
    }

}
