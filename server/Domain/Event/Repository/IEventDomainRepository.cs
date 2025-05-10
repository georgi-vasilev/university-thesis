namespace Domain.Event.Repository
{
    using Common;

    public interface IEventDomainRepository : IDomainRepository<Event>
    {
        Task<IEnumerable<Event>> GetEventsByFilterAsync(Func<Event, bool> predicate,CancellationToken cancellationToken);
        Task<Event> GetEventByFilterAsync(Func<Event, bool> predicate, CancellationToken cancellationToken);

    }
}
