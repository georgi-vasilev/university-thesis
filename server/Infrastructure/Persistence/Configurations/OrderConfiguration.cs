namespace Infrastructure.Persistence.Configurations
{
    using Domain.Order;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(o => o.Id);

            builder.Property(o => o.BuyerId)
                   .IsRequired();

            builder.Property(o => o.Status)
                   .IsRequired()
                   .HasConversion<string>();

            builder.OwnsOne(o => o.Payment, pd =>
            {
                pd.Property(p => p.TransactionId)
                  .HasColumnName("TransactionId")
                  .HasMaxLength(100);

                pd.Property(p => p.Amount)
                  .HasColumnName("Amount");

                pd.Property(p => p.Status)
                  .HasColumnName("PaymentStatus")
                  .HasConversion<string>();
            });

            builder.HasMany(o => o.Tickets)
                .WithOne()
                .HasForeignKey("OrderId")
                .IsRequired();

            builder.Ignore(o => o.DomainEvents);
        }
    }
}