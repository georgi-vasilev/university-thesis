namespace Application.Profile.Command.RegisterBuyer
{
    using Common.Contracts;
    using CreateBuyer;
    using Domain.Buyer.Repository;
    using ErrorOr;
    using MediatR;

    public class RegisterBuyerUserCommandHandler : IRequestHandler<RegisterBuyerUserCommand, ErrorOr<string>>
    {
        private readonly IMediator _mediator;
        private readonly IAuthenticationService _authService;
        private readonly IBuyerDomainRepository _repository;

        public RegisterBuyerUserCommandHandler(
            IMediator mediator,
            IAuthenticationService authService,
            IBuyerDomainRepository repository)
        {
            _mediator = mediator;
            _authService = authService;
            _repository = repository;
        }

        public async Task<ErrorOr<string>> Handle(RegisterBuyerUserCommand request, CancellationToken cancellationToken)
        {
            var createBuyerResult = await _mediator.Send(new CreateBuyerCommand(
                request.FirstName,
                request.LastName,
                request.Email), cancellationToken);

            if (createBuyerResult.IsError)
                return createBuyerResult.Errors;

            var buyer = await _repository.GetByEmailAsync(request.Email, cancellationToken);
            if (buyer is null)
                return Error.NotFound("Buyer", "Buyer not found after creation.");

            return await _authService.CreateBuyerUserAsync(request.Email, request.Password, buyer, cancellationToken);
        }
    }

}
