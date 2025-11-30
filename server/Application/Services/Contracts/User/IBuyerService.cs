namespace Application.Services.Contracts.User
{
    using Domain.Buyer;

    public interface IBuyerService
    {
        Task<Buyer?> GetUserByIdAsync(Guid buyerId, CancellationToken cancellationToken);
    }
}
