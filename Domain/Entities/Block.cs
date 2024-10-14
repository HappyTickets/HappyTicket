using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Block : BaseEntity<long>
{
    public string Name { get; set; }
    public Guid StadiumId { get; set; }
    [ForeignKey(nameof(StadiumId))]
    public virtual StadiumO? Stadium { get; set; }
    public virtual ICollection<SeatO> Seats { get; set; } = new List<SeatO>();
}
