namespace Application.Profile.Command.RegisterHost
{
    using ErrorOr;
    using MediatR;

    public record RegisterHostUserCommand(
        string Username,
        string FirstName,
        string LastName,
        string PhoneNumber,
        string InstagramHandler,
        string Email,
        string Password) : IRequest<ErrorOr<string>>;
}
