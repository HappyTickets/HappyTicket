using Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Block : SoftDeletableEntity<long>
{
    public string Name { get; set; }
    public long StadiumId { get; set; }
    [ForeignKey(nameof(StadiumId))]
    public virtual Stadium? Stadium { get; set; }
    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
}
