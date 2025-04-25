namespace Infrastructure.Persistence
{
    using Domain.Event;
    using Domain.Host;
    using Domain.Order;
    using Microsoft.EntityFrameworkCore;

    internal interface IApplicationDbContext
    {
        DbSet<Event> Events { get; }
        DbSet<Host> Hosts { get; }
        DbSet<Order> Orders { get; }
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
