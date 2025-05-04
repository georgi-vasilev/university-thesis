namespace Infrastructure.Persistence.Configurations
{
    using Authentication;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("AspNetUsers");

            builder.HasOne(u => u.Host)
                   .WithOne()
                   .HasForeignKey<ApplicationUser>(u => u.HostId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(u => u.Buyer)
                   .WithOne()
                   .HasForeignKey<ApplicationUser>(u => u.BuyerId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

