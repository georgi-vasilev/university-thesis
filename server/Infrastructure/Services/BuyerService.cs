namespace Infrastructure.Services
{
    using Application.Services.Contracts.User;
    using Domain.Buyer;
    using Domain.Buyer.Repository;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    internal class BuyerService : IBuyerService
    {
        private readonly IBuyerDomainRepository _repo;

        public BuyerService(IBuyerDomainRepository repo)
        {
            _repo = repo;
        }
        public async Task<Buyer?> GetUserByIdAsync(Guid buyerId, CancellationToken cancellationToken) 
            => await _repo.GetUserByIdAsync(buyerId, cancellationToken);
    }
}
