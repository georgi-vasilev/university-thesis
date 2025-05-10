namespace Application.Profile.Command.RegisterHost
{
    using Common.Contracts;
    using Domain.Host.Repository;
    using ErrorOr;
    using Host.Commands.Create;
    using MediatR;

    public class RegisterHostUserCommandHandler : IRequestHandler<RegisterHostUserCommand, ErrorOr<string>>
    {
        private readonly IMediator _mediator;
        private readonly IHostDomainRepository _hostRepository; // or domain repo if needed
        private readonly IAuthenticationService _authService;

        public RegisterHostUserCommandHandler(
            IMediator mediator,
            IHostDomainRepository hostQueryRepository,
            IAuthenticationService authService)
        {
            _mediator = mediator;
            _hostRepository = hostQueryRepository;
            _authService = authService;
        }

        public async Task<ErrorOr<string>> Handle(RegisterHostUserCommand request, CancellationToken cancellationToken)
        {
            var createHostResult = await _mediator.Send(new CreateHostCommand(
                request.FirstName,
                request.LastName,
                request.PhoneNumber,
                request.Email,
                request.InstagramHandler), cancellationToken);

            if (createHostResult.IsError)
            {
                return createHostResult.Errors;
            }

            var host = await _hostRepository.GetHostByEmailAsync(request.Email, cancellationToken);
            if (host is null)
            {
                return Error.NotFound("Host", "Host not found after creation.");
            }

            return await _authService.CreateHostUserAsync(request.Email, request.Password, host, cancellationToken);
        }
    }

}
