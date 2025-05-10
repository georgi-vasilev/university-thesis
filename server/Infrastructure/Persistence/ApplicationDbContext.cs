namespace Infrastructure.Persistence
{
    using Domain.Buyer;
    using Domain.Event;
    using Domain.Host;
    using Domain.Order;
    using Domain.Venue;
    using Infrastructure.Authentication;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Event> Events { get; set; } = default!;
        public DbSet<Host> Hosts { get; set; } = default!;
        public DbSet<Order> Orders { get; set; } = default!;
        public DbSet<Ticket> Tickets { get; set; } = default!;
        public DbSet<Venue> Venues { get; set; } = default!;
        public DbSet<Buyer> Buyers { get; set; } = default!;

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // TODO: Dispatch any domain events from aggregates before or after persisting
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
