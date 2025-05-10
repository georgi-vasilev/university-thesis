namespace Infrastructure.Persistence.Configurations
{
    using Domain.Buyer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class BuyerConfiguration : IEntityTypeConfiguration<Buyer>
    {
        public void Configure(EntityTypeBuilder<Buyer> builder)
        {
            builder.ToTable("Buyers");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.FirstName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(b => b.LastName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(b => b.Email)
                   .IsRequired()
                   .HasMaxLength(256);

            builder.HasIndex(b => b.Email)
                   .IsUnique();

        }
    }
}
