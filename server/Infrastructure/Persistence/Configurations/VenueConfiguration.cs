namespace Infrastructure.Persistence.Configurations
{
    using Domain.Venue;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;

    internal class VenueConfiguration : IEntityTypeConfiguration<Venue>
    {
        public void Configure(EntityTypeBuilder<Venue> builder)
        {
            builder.ToTable("Venues");
            builder.HasKey(v => v.Id);

            builder.Property(v => v.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(v => v.Capacity)
                   .IsRequired();

            builder.Property(v => v.Type)
                   .IsRequired()
                   .HasConversion<string>();

            builder.OwnsOne(v => v.Address, addr =>
            {
                addr.Property(a => a.Street)
                    .HasColumnName("Street")
                    .HasMaxLength(200);
                addr.Property(a => a.City)
                    .HasColumnName("City")
                    .HasMaxLength(100);
                addr.Property(a => a.State)
                    .HasColumnName("State")
                    .HasMaxLength(50);
                addr.Property(a => a.Country)
                    .HasColumnName("Country")
                    .HasMaxLength(50);
                addr.Property(a => a.ZipCode)
                    .HasColumnName("ZipCode")
                    .HasMaxLength(20);
            });
        }
    }
}
