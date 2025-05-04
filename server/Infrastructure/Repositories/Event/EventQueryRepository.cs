namespace Infrastructure.Repositories.Event
{
    using Application.Common.Contracts;
    using Application.Events.Queries.GetEventDetails;
    using Application.Events.Queries.GetEvents;
    using Domain.Event.Error;
    using ErrorOr;
    using Infrastructure.Persistence;
    using Microsoft.EntityFrameworkCore;

    public class EventQueryRepository : IEventQueryRepository
    {
        private IApplicationDbContext _context;

        public EventQueryRepository(IApplicationDbContext context) => _context = context;

        public async Task<ErrorOr<GetEventDetailsOutputModel>> GetDetailsAsync(Guid eventId, CancellationToken cancelletionToken)
        {
            var eventDetails = await (
                from e in _context.Events.AsNoTracking()
                join v in _context.Venues.AsNoTracking()
                    on e.VenueId equals v.Id
                join h in _context.Hosts.AsNoTracking()
                    on e.HostId equals h.Id
                where e.Id == eventId
                select new GetEventDetailsOutputModel(
                e.Id,
                e.Name,
                e.Description,
                v.Name,
                h.ContactInfo.FullName,
                e.Date,
                e.Time,
                e.Status.ToString(),
                v.Capacity - e.TicketCount
                )).FirstOrDefaultAsync(cancelletionToken);

            if (eventDetails is null)
            {
                return EventErrors.NotFoundError;
            }

            return eventDetails;
        }

        public async Task<ErrorOr<List<GetEventsOutputModel>>> GetEventsByHostAsync(Guid hostId, CancellationToken cancelletionToken)
        {
            var eventDetails = await(
                from e in _context.Events.AsNoTracking()
                join h in _context.Hosts.AsNoTracking()
                    on e.HostId equals h.Id
                where e.HostId == hostId
                select new GetEventsOutputModel(
                e.Id,
                e.Name,
                e.Description,
                e.Date,
                e.Time,
                e.Status.ToString()
                )).ToListAsync(cancelletionToken);

            if (eventDetails.Count == 0)
            {
                return EventErrors.GetEventsByHostNotFoundError;
            }

            return eventDetails;
        }

        public async Task<ErrorOr<List<GetEventsOutputModel>>> GetEventsByVenueAsync(Guid venueId, CancellationToken cancelletionToken)
        {
            var eventDetails = await(
              from e in _context.Events.AsNoTracking()
              join h in _context.Hosts.AsNoTracking()
                  on e.HostId equals h.Id
              where e.VenueId == venueId
              select new GetEventsOutputModel(
              e.Id,
              e.Name,
              e.Description,
              e.Date,
              e.Time,
              e.Status.ToString()
              )).ToListAsync(cancelletionToken);

            if (eventDetails.Count == 0)
            {
                return EventErrors.GetEventsByVenueNotFoundError;
            }

            return eventDetails;
        }
    }
}
