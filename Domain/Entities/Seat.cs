using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;

namespace Domain.Entities;

public class Seat : SoftDeletableEntity<long>
{
    public string Number { get; set; }
    public long BlockId { get; set; }
    [ForeignKey(nameof(BlockId))]
    public virtual Block? Block { get; set; }
}
