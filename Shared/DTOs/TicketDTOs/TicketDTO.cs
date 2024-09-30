using Shared.DTOs.MatchDtos;
using Shared.Enums;

namespace Shared.DTOs.TicketDTOs;

public class TicketDto
{
    public Guid Id { get; set; }
    public string Class { get; set; }
    public decimal Price { get; set; }
    public Guid TeamId { get; set; }
    public TeamDto? Team { get; set; }
    public string Location { get; set; }
    public bool DisplayForSale { get; set; }
    public int TicketsCount { get; set; }
    public Guid MatchId { get; set; }
    public MatchDto? Match { get; set; }
    public TicketStatusDTO TicketStatus{ get; set; }

    public bool IsForAdmins { get; set; }
    public string ExternalGate { get; set; } 
    public string InternalGate { get; set; } 

}
