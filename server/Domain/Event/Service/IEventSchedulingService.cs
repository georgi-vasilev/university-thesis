namespace Domain.Event.Service
{
    using ErrorOr;

    public interface IEventSchedulingService
    {
        Task<ErrorOr<Success>> ValidateNewEventAsync(
            Guid venueId,
            DateOnly date,
            TimeRange newEventTime,
            CancellationToken cancellationToken);
    }
}
