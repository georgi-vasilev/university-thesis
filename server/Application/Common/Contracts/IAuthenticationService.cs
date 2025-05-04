namespace Application.Common.Contracts
{
    using Domain.Buyer;
    using Domain.Host;
    using ErrorOr;

    public interface IAuthenticationService
    {
        Task<ErrorOr<string>> LoginAsync(string email, string password);

        Task<ErrorOr<string>> CreateHostUserAsync(
            string email,
            string password,
            Host host,
            CancellationToken cancellationToken);

        Task<ErrorOr<string>> CreateBuyerUserAsync(
            string email,
            string password,
            Buyer buyer, 
            CancellationToken cancellationToken);
    }
}

