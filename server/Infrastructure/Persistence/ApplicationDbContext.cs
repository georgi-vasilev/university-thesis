namespace Infrastructure.Persistence
{
    using Domain.Event;
    using Domain.Host;
    using Domain.Order;
    using Microsoft.EntityFrameworkCore;

    internal class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public DbSet<Event> Events { get; set; } = default!;
        public DbSet<Host> Hosts { get; set; } = default!;
        public DbSet<Order> Orders { get; set; } = default!;

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // TODO: Dispatch any domain events from aggregates before or after persisting
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //TODO: Add configuration and apply all IEntityTypeConfiguration<> from this assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
