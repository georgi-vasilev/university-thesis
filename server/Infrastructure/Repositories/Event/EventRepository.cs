namespace Infrastructure.Repositories.Event
{
    using Application.Common.Contracts;
    using Application.Events.Queries.GetEventDetails;
    using Application.Events.Queries.GetEvents;
    using Azure.Core;
    using Domain.Event;
    using Domain.Event.Error;
    using Domain.Event.Repository;
    using ErrorOr;
    using Microsoft.EntityFrameworkCore;
    using Persistence;

    public class EventRepository : IEventDomainRepository
    {
        private readonly IApplicationDbContext _context;

        public EventRepository(IApplicationDbContext context)
            => _context = context;

        public async Task AddAsync(Event aggregate, CancellationToken cancellationToken)
        {
            _context.Events.Add(aggregate);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await _context.Events.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (entity is not null)
            {
                _context.Events.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<Event?> GetEventByFilterAsync(Func<Event, bool> predicate, CancellationToken cancellationToken)
        {
            var all = await _context
                .Events
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return all.FirstOrDefault(predicate);
        }

        public async Task<IEnumerable<Event>> GetEventsByFilterAsync(Func<Event, bool> predicate, CancellationToken cancellationToken)
        {
            var all = await _context
                .Events
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return all.Where(predicate);
        }

        public async Task UpdateAsync(Event aggregate, CancellationToken cancellationToken)
        {
            _context.Events.Update(aggregate);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}