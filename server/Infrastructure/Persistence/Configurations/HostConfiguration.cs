namespace Infrastructure.Persistence.Configurations
{
    using Domain.Host;
    using Domain.Venue;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System.Text.Json;

    internal class HostConfiguration : IEntityTypeConfiguration<Host>
    {
        public void Configure(EntityTypeBuilder<Host> builder)
        {
            builder.ToTable("Hosts");
            builder.HasKey(h => h.Id);

            builder.Property(h => h.VenueId)
                .IsRequired(false);

            builder.HasOne<Venue>()
                .WithMany()
                .HasForeignKey(h => h.VenueId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.OwnsOne(h => h.ContactInfo, ci =>
            {
                ci.Property(c => c.FirstName)
                  .HasColumnName("FirstName")
                  .IsRequired()
                  .HasMaxLength(50);

                ci.Property(c => c.LastName)
                  .HasColumnName("LastName")
                  .IsRequired()
                  .HasMaxLength(50);

                ci.Property(c => c.PhoneNumber)
                  .HasColumnName("PhoneNumber")
                  .HasMaxLength(20);

                ci.Property(c => c.Email)
                  .HasColumnName("Email")
                  .HasMaxLength(100);

                ci.Property(c => c.InstagramHandler)
                  .HasColumnName("InstagramHandler")
                  .HasMaxLength(50);
            });

            builder.Property<List<Guid>>("_organizedEventIds")
                .HasColumnName("OrganizedEventIds")
                .HasConversion(
                    v => JsonSerializer.Serialize(v ?? new List<Guid>(), (JsonSerializerOptions)null),
                    v => string.IsNullOrWhiteSpace(v)
                        ? new List<Guid>()
                        : JsonSerializer.Deserialize<List<Guid>>(v, (JsonSerializerOptions)null))
                .HasColumnType("nvarchar(max)");

            builder.Ignore(h => h.DomainEvents);
        }
    }

}
