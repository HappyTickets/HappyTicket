using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Block : BaseEntity
    {
        public string Name { get; set; }
        public Guid StadiumId { get; set; }
        [ForeignKey(nameof(StadiumId))]
        public virtual Stadium? Stadium { get; set; }
        public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
    }

}
