namespace Application.Profile.Command.CreateBuyer
{
    using ErrorOr;
    using MediatR;

    public record CreateBuyerCommand(
        string FirstName,
        string LastName,
        string Email) : IRequest<ErrorOr<Guid>>;
}
