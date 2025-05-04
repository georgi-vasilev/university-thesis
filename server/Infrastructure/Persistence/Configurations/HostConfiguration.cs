namespace Infrastructure.Persistence.Configurations
{
    using Domain.Host;
    using Domain.Venue;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

            builder.Ignore(h => h.DomainEvents);
        }
    }

}
