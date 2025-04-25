namespace Infrastructure.Repositories
{
    using Domain.Event;
    using Domain.Event.Repository;

    public class EventRepository : IEventDomainRepository
    {
        public Task AddAsync(Event aggregate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Event> GetEventByFilterAsync(Func<Event, bool> predicate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Event>> GetEventsByFilter(Func<Event, bool> predicate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Event aggregate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
