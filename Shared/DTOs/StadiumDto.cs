﻿namespace Shared.DTOs
{
    public class StadiumDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public List<BlockDto> Blocks { get; set; } = new List<BlockDto>();
    }
}
