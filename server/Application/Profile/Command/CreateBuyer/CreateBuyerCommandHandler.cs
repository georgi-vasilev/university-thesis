namespace Application.Profile.Command.CreateBuyer
{
    using Domain.Buyer.Builder;
    using Domain.Buyer.Repository;
    using ErrorOr;
    using MediatR;

    public class CreateBuyerCommandHandler : IRequestHandler<CreateBuyerCommand, ErrorOr<Guid>>
    {
        private readonly IBuyerDomainRepository _repository;
        private readonly IBuyerBuilder _buyerBuilder;

        public CreateBuyerCommandHandler(
            IBuyerDomainRepository repository,
            IBuyerBuilder buyerBuilder)
        {
            _repository = repository;
            _buyerBuilder = buyerBuilder;
        }

        public async Task<ErrorOr<Guid>> Handle(CreateBuyerCommand request, CancellationToken cancellationToken)
        {
            var buyerResult = _buyerBuilder
                .WithName(request.FirstName, request.LastName)
                .WithEmail(request.Email)
                .Build();

            if (buyerResult.IsError)
                return buyerResult.FirstError;

            var buyer = buyerResult.Value;
            await _repository.AddAsync(buyer, cancellationToken);

            return buyer.Id;
        }
    }

}
