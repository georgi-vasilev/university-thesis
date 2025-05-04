namespace Infrastructure.Persistence
{
    using Domain.Buyer;
    using Domain.Common;
    using Domain.Event;
    using Domain.Host;
    using Domain.Order;
    using Domain.Venue;
    using Infrastructure.Authentication;
    using Infrastructure.Middleware;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            IHttpContextAccessor httpContextAccessor)
        : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<Event> Events { get; set; } = default!;
        public DbSet<Host> Hosts { get; set; } = default!;
        public DbSet<Order> Orders { get; set; } = default!;
        public DbSet<Ticket> Tickets { get; set; } = default!;
        public DbSet<Venue> Venues { get; set; } = default!;
        public DbSet<Buyer> Buyers { get; set; } = default!;

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var domainEvents = ChangeTracker.Entries<IAggregateRoot>()
                .SelectMany(e => e.Entity.DomainEvents)
                .ToList();

            var result = await base.SaveChangesAsync(cancellationToken);


            Queue<IDomainEvent> domainEventsQueue = _httpContextAccessor.HttpContext.Items.TryGetValue(EventualConsistencyMiddleware.DomainEventsKey, out var value) &&
                value is Queue<IDomainEvent> existingDomainEvents
                    ? existingDomainEvents
                    : new();

            domainEvents.ForEach(domainEventsQueue.Enqueue);
            _httpContextAccessor.HttpContext.Items[EventualConsistencyMiddleware.DomainEventsKey] = domainEventsQueue;
            return result;
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
