using Application.Common.Implementations;
using Application.Common.Interfaces.Persistence;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Shared.Common;
using Shared.Common.General;
using Shared.DTOs.Sponsors;
using Shared.Exceptions;
using System.Net;

namespace Application.Implementations
{
    public class SponsorService : BaseService<Sponsor>, ISponsorService
    {
        public SponsorService(IUnitOfWork unitOfWork, ILogger<Sponsor> logger, IMapper mapper) : base(unitOfWork, logger, mapper)
        {
        }

        public async Task<BaseResponse<long>> CreateAsync(CreateOrUpdateSponsorDto dto, CancellationToken cancellationToken = default)
        {
            var sponsor = _mapper.Map<Sponsor>(dto);

            _unitOfWork.Repository<Sponsor>().Create(sponsor);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return sponsor.Id;
        }

        public async Task<BaseResponse<Empty>> UpdateAsync(long id, CreateOrUpdateSponsorDto dto, CancellationToken cancellationToken = default)
        {
            var sponsorRepo = _unitOfWork.Repository<Sponsor>();

            var sponsor = await sponsorRepo.GetByIdAsync(id, cancellationToken: cancellationToken);
            if (sponsor == null)
                return new NotFoundException();

            _mapper.Map(dto, sponsor);

            sponsorRepo.Update(sponsor);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Empty.Default;
        }

        public async Task<BaseResponse<Empty>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var sponsorRepo = _unitOfWork.Repository<Sponsor>();

            var sponsor = await sponsorRepo.GetByIdAsync(id, cancellationToken: cancellationToken);

            if (sponsor == null)
                return new NotFoundException();

            sponsorRepo.SoftDelete(sponsor);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Empty.Default;
        }
        
        public async Task<BaseResponse<Empty>> HardDeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var sponsorRepo = _unitOfWork.Repository<Sponsor>();

            var sponsor = await sponsorRepo.GetByIdAsync(id, cancellationToken: cancellationToken);

            if (sponsor == null)
                return new NotFoundException();

            sponsorRepo.HardDelete(sponsor);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Empty.Default;
        }

        public async Task<BaseResponse<SponsorDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var sponsorRepo = _unitOfWork.Repository<Sponsor>();

            var sponsor = await sponsorRepo.GetByIdAsync(id, cancellationToken: cancellationToken);

            if (sponsor == null)
                return new NotFoundException();

            return _mapper.Map<SponsorDto>(sponsor);
        }
        
        public async Task<BaseResponse<IEnumerable<SponsorDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var sponsorRepo = _unitOfWork.Repository<Sponsor>();

            var sponsors = await sponsorRepo.ListAsync(cancellationToken: cancellationToken); 

            return _mapper.Map<List<SponsorDto>>(sponsors);
        }
    }
}
