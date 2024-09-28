//using Application.Interfaces.Persistence;
//using AutoMapper;
//using Domain.Entities;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Shared.DTOs.TicketDTOs;
//using System.Linq.Expressions;

//namespace API.Controllers;

//public class BlockController : BaseController
//{
//    private readonly IUnitOfWork _unitOfWork;
//    private readonly IMapper mapper;

//    public BlockController(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IMapper mapper) : base(httpContextAccessor)
//    {
//        _unitOfWork = unitOfWork;
//        this.mapper = mapper;
//    }

//    [HttpGet("GetTAll")]
//    public async Task<IActionResult> GetTAll(CancellationToken cancellationToken = default)
//    {
//        return ReturnResult(await _unitOfWork.Repository<Ticket>().GetAllAsync(cancellationToken, x => x.Include(x => x.Block)!));
//    }

//    [HttpGet("GetAll")]
//    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
//    {
//        return ReturnResult(await _unitOfWork.Repository<Block>().GetAllAsync(cancellationToken, x => x.Include(x => x.Tickets)!));
//    }

//    [HttpGet("Get")]
//    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken = default)
//    {
//        return ReturnResult(await _unitOfWork.Repository<Block>().GetByIdAsync(id, cancellationToken, x => x.Include(x => x.Tickets)!));
//    }

//    [HttpPost("Add")]
//    public async Task<IActionResult> Add(BlockDTO blockDTO, CancellationToken cancellationToken = default)
//    {
//        var res = ReturnResult(await _unitOfWork.Repository<Block>().CreateAsync(new() { BlockNumber = blockDTO.BlockNumber, Tickets = blockDTO.TicketsDTO?.Select(x => new Ticket() { SerialNumber = x.SeatNumber, Description = x.Description, SeatNumber = x.SeatNumber, QRCodeUrl = x.QRCodeUrl, BarCodeUrl = x.BarCodeUrl, TicketStatus = x.TicketStatus, EventId = x.EventId, PurchaseInvoiceId = x.PurchaseInvoiceId, RatingId = x.RatingId, OwnerId = x.OwnerId }) }, cancellationToken));
//        await _unitOfWork.SaveChangesAsync();
//        return res;
//    }

//    [HttpGet("Recover")]
//    public async Task<IActionResult> Recover(Guid id, CancellationToken cancellationToken = default)
//    {
//        return ReturnResult(await _unitOfWork.Repository<Block>().RecoverByIdAsync(id, cancellationToken));
//    }

//    [HttpGet("SoftDelete")]
//    public async Task<IActionResult> SoftDelete(Guid id, CancellationToken cancellationToken = default)
//    {
//        return ReturnResult(await _unitOfWork.Repository<Block>().SoftDeleteByIdAsync(id, cancellationToken));
//    }

//    [HttpGet("HardDelete")]
//    public async Task<IActionResult> HardDelete(Guid id, CancellationToken cancellationToken = default)
//    {
//        return ReturnResult(await _unitOfWork.Repository<Block>().HardDeleteByIdAsync(id, cancellationToken));
//    }
//}
