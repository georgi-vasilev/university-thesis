namespace Domain.Event.Service
{
    using Common;
    using ErrorOr;
    using Host;

    public interface IEventHostService : IDomainService
    {
        Task<ErrorOr<Success>> CreateEventForHostAsync(Host host, Event @event, CancellationToken cancellationToken);
        Task<ErrorOr<Success>> RemoveEventFromHostAsync(Host host, Guid eventId, CancellationToken cancellationToken);
        Task<ErrorOr<Success>> CancelEventForHostAsync(Host host, Guid eventId, CancellationToken cancellationToken);
    }
}
