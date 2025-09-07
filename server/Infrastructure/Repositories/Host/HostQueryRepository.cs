namespace Infrastructure.Repositories.Host
{
    using Application.Common.Contracts;
    using Application.Events.Queries.GetEventDetails;
    using Application.Host.Queries;
    using Domain.Host.Error;
    using ErrorOr;
    using Microsoft.EntityFrameworkCore;
    using Persistence;

    public class HostQueryRepository : IHostQueryRepository
    {
        private readonly IApplicationDbContext _context;

        public HostQueryRepository(IApplicationDbContext db)
            => _context = db;

        public async Task<ErrorOr<GetHostDetailsOutputModel>> GetDetailsAsync(
            Guid hostId,
            CancellationToken cancellationToken)
        {
            var hostDto = await _context.Hosts
                .AsNoTracking()
                .Where(h => h.Id == hostId)
                .Select(h => new
                {
                    h.Id,
                    h.ContactInfo.FullName,
                    h.ContactInfo.Email,
                    h.ContactInfo.InstagramHandler
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (hostDto is null)
                return HostErrors.HostNotFoundError;

            var events = await (
                from e in _context.Events.AsNoTracking()
                join v in _context.Venues.AsNoTracking()
                  on e.VenueId equals v.Id
                where e.HostId == hostId
                select new GetEventDetailsOutputModel(
                    e.Id,
                    e.Name,
                    e.Description,
                    e.ImageUrl,
                    v.Name,
                    hostDto.Id,
                    hostDto.FullName,
                    e.Date,
                    e.Time,
                    e.GeneralPrice.Amount,
                    e.VipPrice.Amount,
                    e.Status.ToString(),
                    v.Capacity - e.TicketCount
                )
            ).ToListAsync(cancellationToken);

            var result = new GetHostDetailsOutputModel(
                hostDto.FullName,
                hostDto.Email,
                hostDto.InstagramHandler,
                events);

            return result;
        }
    }
}
