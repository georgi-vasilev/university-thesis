namespace Application.Profile.Command.Login
{
    using ErrorOr;
    using MediatR;

    public record LoginUserCommand(
        string Email,
        string Password) : IRequest<ErrorOr<string>>;
}
