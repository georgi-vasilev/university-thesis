namespace Application.Common.Contracts
{
    using ErrorOr;
    using Events.Queries.GetEventDetails;
    using Events.Queries.GetEvents;

    public interface IEventQueryRepository
    {
        Task<ErrorOr<GetEventDetailsOutputModel>> GetDetailsAsync(Guid eventId, CancellationToken cancelletionToken);
        Task<ErrorOr<List<GetEventsOutputModel>>> GetEventsByHostAsync(Guid hostId, CancellationToken cancelletionToken);
        Task<ErrorOr<List<GetEventsOutputModel>>> GetEventsByVenueAsync(Guid venueId, CancellationToken cancelletionToken);
    }
}
