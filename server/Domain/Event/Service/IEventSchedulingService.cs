namespace Domain.Event.Service
{
    using Common;
    using ErrorOr;

    public interface IEventSchedulingService : IDomainService
    {
        Task<ErrorOr<Success>> ValidateNewEventAsync(
            Guid venueId,
            DateOnly date,
            TimeRange newEventTime,
            CancellationToken cancellationToken);
    }
}
