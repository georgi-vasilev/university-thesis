namespace Infrastructure.Authentication
{
    using Domain.Host;
    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser, IUser
    {
        internal User(string email)
            : base(email)
            => this.Email = email;

        public Host? Host { get; private set; }

        public void BecomeHost(Host host)
        {
            if (this.Host != null)
            {
                throw new InvalidDataException($"User '{this.UserName}' is already a host.");
            }

            this.Host = host;
        }
    }
}

