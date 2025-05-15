namespace Application.Common.Contracts
{
    using Application.Events.Common;
    using Domain.Event;
    using ErrorOr;
    using Events.Queries.GetEventDetails;
    using Events.Queries.GetEvents;
    using Models;

    public interface IEventQueryRepository
    {
        Task<ErrorOr<GetEventDetailsOutputModel>> GetDetailsAsync(Guid eventId, CancellationToken cancelletionToken);
        Task<ErrorOr<PaginatedResult<GetEventsOutputModel>>> GetEventsByHostAsync(
            Guid hostId,
            int pageIndex,
            int pageSize,
            EventOrdering ordering,
            CancellationToken cancellationToken);
        Task<ErrorOr<List<GetEventsOutputModel>>> GetEventsByVenueAsync(Guid venueId, CancellationToken cancelletionToken);
        Task<PaginatedResult<Event>> GetActiveEventsAsync(
            int pageIndex,
            int pageSize,
            EventOrdering ordering,
            CancellationToken cancellationToken);

        Task<PaginatedResult<Event>> GetEventsInDateRangeAsync(
            DateTime startDateUtc,
            DateTime endDateUtc,
            int pageIndex,
            int pageSize,
            EventOrdering ordering,
            CancellationToken cancellationToken);
    }
}
