namespace Infrastructure.Repositories.Event
{
    using Application.Common.Contracts;
    using Application.Common.Models;
    using Application.Events.Common;
    using Application.Events.Queries.GetEventDetails;
    using Application.Events.Queries.GetEvents;
    using Domain.Event;
    using Domain.Event.Error;
    using ErrorOr;
    using Infrastructure.Persistence;
    using Microsoft.EntityFrameworkCore;

    public class EventQueryRepository : IEventQueryRepository
    {
        private IApplicationDbContext _context;

        public EventQueryRepository(IApplicationDbContext context) => _context = context;

        public async Task<PaginatedResult<Event>> GetActiveEventsAsync(
            int pageIndex,
            int pageSize,
            EventOrdering ordering,
            CancellationToken cancellationToken)
        {
            var query = _context.Events
                .Where(e => e.Status == EventStatus.Active);

            query = ApplyOrdering(query, ordering);

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedResult<Event>(items, pageIndex, pageSize, totalCount);
        }

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
                h.ContactInfo.InstagramHandler,
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

            if(eventDetails is null)
            {
                return EventErrors.GetEventsByHostNotFoundError;
            }

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

        public async Task<PaginatedResult<Event>> GetEventsInDateRangeAsync(
            DateTime startDateUtc,
            DateTime endDateUtc,
            int pageIndex,
            int pageSize,
            EventOrdering ordering,
            CancellationToken cancellationToken)
        {
            var query = _context.Events
                .Where(e => e.Time.Start >= startDateUtc && e.Time.End <= endDateUtc);

            query = ApplyOrdering(query, ordering);

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedResult<Event>(items, pageIndex, pageSize, totalCount);
        }

        private IQueryable<Event> ApplyOrdering(IQueryable<Event> query, EventOrdering ordering)
        {
            return ordering switch
            {
                EventOrdering.DateAsc => query.OrderBy(e => e.Date).ThenBy(e => e.Time.Start),
                EventOrdering.DateDesc => query.OrderByDescending(e => e.Date).ThenByDescending(e => e.Time.Start),
                EventOrdering.NameAsc => query.OrderBy(e => e.Name),
                EventOrdering.NameDesc => query.OrderByDescending(e => e.Name),
                _ => query.OrderBy(e => e.Date)
            };
        }

    }
}
