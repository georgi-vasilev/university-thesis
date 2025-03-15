namespace Domain.Event.Service
{
    using ErrorOr;
    using Host;

    public interface IEventHostService
    {
        Task<ErrorOr<Success>> CreateEventForHostAsync(Host host, Event @event);
        Task<ErrorOr<Success>> RemoveEventFromHostAsync(Host host, Guid eventId);
        Task<ErrorOr<Success>> CancelEventForHostAsync(Host host, Guid eventId);
    }
}
