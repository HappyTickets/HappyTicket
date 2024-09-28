namespace Domain.Entities
{
    public class Stadium : BaseEntity
    {
        public string Name { get; set; }
        public string? Photo { get; set; }
        public string? location { get; set; }
        public virtual ICollection<Block>? Blocks { get; set; } = new List<Block>();
    }

}
