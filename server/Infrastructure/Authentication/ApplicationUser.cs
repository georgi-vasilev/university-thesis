namespace Infrastructure.Authentication
{
    using Microsoft.AspNetCore.Identity;
    using Domain.Host;
    using Domain.Buyer;

    public class ApplicationUser : IdentityUser, IUser
    {
        // Required for Identity + EF
        public ApplicationUser() { } 

        internal ApplicationUser(string email)
            : base(email)
        {
            Email = email;
        }

        public Host? Host { get; set; }
        public Buyer? Buyer { get; set; }

        public Guid? HostId { get; set; }
        public Guid? BuyerId { get; set; }

        public void BecomeHost(Host host)
        {
            if (Host != null)
                throw new InvalidDataException($"User '{UserName}' is already a host.");

            Host = host;
            HostId = host.Id;
        }

        public void RegisterAsBuyer(Guid buyerId)
        {
            if (BuyerId != null)
                throw new InvalidDataException($"User '{UserName}' is already a buyer.");

            BuyerId = buyerId;
        }
    }
}
