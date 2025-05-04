namespace Infrastructure.Persistence.Configurations
{
    using Domain.Event;
    using Domain.Host;
    using Domain.Venue;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable("Events");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(e => e.Description)
                   .HasMaxLength(500);

            builder.Property(e => e.Date)
                   .HasColumnType("date");

            builder.Property(e => e.Status)
                   .IsRequired()
                   .HasConversion<string>();

            builder.Property(e => e.VenueId)
                   .IsRequired();

            builder.Property(e => e.HostId)
                   .IsRequired();

            builder.HasOne<Host>()
                   .WithMany()
                   .HasForeignKey(e => e.HostId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Venue>()
                   .WithMany()
                   .HasForeignKey(e => e.VenueId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.OwnsOne(e => e.Time, tr =>
            {
                tr.Property(t => t.Start)
                  .HasColumnName("StartTime");
                tr.Property(t => t.End)
                  .HasColumnName("EndTime");
            });

            builder.OwnsOne(e => e.GeneralPrice, m =>
            {
                m.Property(p => p.Amount).HasColumnName("GeneralPriceAmount");
                m.Property(p => p.Currency).HasColumnName("GeneralPriceCurrency").HasMaxLength(3);
            });
            builder.OwnsOne(e => e.VipPrice, m =>
            {
                m.Property(p => p.Amount).HasColumnName("VipPriceAmount");
                m.Property(p => p.Currency).HasColumnName("VipPriceCurrency").HasMaxLength(3);
            });

            builder.Ignore(e => e.DomainEvents);
        }
    }
}
