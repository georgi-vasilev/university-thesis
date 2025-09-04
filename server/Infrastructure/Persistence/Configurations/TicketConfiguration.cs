namespace Infrastructure.Persistence.Configurations
{
    using Domain.Event;
    using Domain.Order;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.ToTable("Tickets");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.EventId)
                   .IsRequired();

            builder.HasOne<Event>()
                .WithMany()
                .HasForeignKey(t => t.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Order>()
                .WithMany(o => o.Tickets)
                .HasForeignKey("OrderId")
                .IsRequired();

            builder.OwnsOne(t => t.Price, m =>
            {
                m.Property(x => x.Amount)
                 .HasColumnName("PriceAmount")
                 .IsRequired();

                m.Property(x => x.Currency)
                 .HasColumnName("PriceCurrency")
                 .HasMaxLength(3)
                 .IsRequired();
            });

            builder.Navigation(t => t.Price).IsRequired();

            builder.Property(t => t.Type)
                   .IsRequired()
                   .HasConversion<string>();

            builder.Property(t => t.Status)
                   .IsRequired()
                   .HasConversion<string>();

            builder.Property(t => t.AttendeeId);

            builder.Property<Guid>("OrderId");
        }
    }
}
