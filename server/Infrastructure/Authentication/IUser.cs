namespace Infrastructure.Authentication
{
    using Domain.Host;
    public interface IUser
    {
        void BecomeHost(Host host);
    }
}
