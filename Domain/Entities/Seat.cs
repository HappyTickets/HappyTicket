using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Seat : BaseEntity<long>
{
    public string Number { get; set; }
    public Guid BlockId { get; set; }
    [ForeignKey(nameof(BlockId))]
    public virtual BlockO? Block { get; set; }
}
