namespace Application.Profile.Command.RegisterBuyer
{
    using ErrorOr;
    using MediatR;

    public record RegisterBuyerUserCommand(
        string FirstName,
        string LastName,
        string Email,
        string Password) : IRequest<ErrorOr<string>>;

}
