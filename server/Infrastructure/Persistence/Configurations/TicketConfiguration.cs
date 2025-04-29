namespace Infrastructure.Persistence.Configurations
{
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

            builder.OwnsOne(t => t.Price, m =>
            {
                m.Property(x => x.Amount)
                  .HasColumnName("PriceAmount");
                m.Property(x => x.Currency)
                  .HasColumnName("PriceCurrency")
                  .HasMaxLength(3);
            });

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
