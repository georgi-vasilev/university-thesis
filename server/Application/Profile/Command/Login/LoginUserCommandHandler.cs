namespace Application.Profile.Command.Login
{
    using Common.Contracts;
    using ErrorOr;
    using MediatR;

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, ErrorOr<string>>
    {
        private readonly IAuthenticationService _auth;

        public LoginUserCommandHandler(IAuthenticationService auth) => _auth = auth;

        public Task<ErrorOr<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
            => _auth.LoginAsync(request.Email, request.Password);
    }
}
