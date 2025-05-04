namespace Application.Host.Commands.Create
{
    using ErrorOr;
    using MediatR;

    public record CreateHostCommand(
        string FirstName,
        string LastName,
        string PhoneNumber,
        string Email,
        string InstagramHandler) : IRequest<ErrorOr<CreateHostOutputModel>>;
}
