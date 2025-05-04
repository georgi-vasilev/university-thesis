namespace Infrastructure.Persistence
{
    using Domain.Event;
    using Domain.Host;
    using Domain.Order;
    using Domain.Venue;
    using Microsoft.EntityFrameworkCore;

    public interface IApplicationDbContext
    {
        DbSet<Event> Events { get; }
        DbSet<Host> Hosts { get; }
        DbSet<Order> Orders { get; }
        DbSet<Venue> Venues { get; }
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
