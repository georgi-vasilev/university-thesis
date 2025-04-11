namespace Application.Host.Commands.Create
{
    using ErrorOr;
    using MediatR;

    public record CreateHostCommand : IRequest<ErrorOr<CreateHostOutputModel>>
    {
        public string FirstName { get; init; } = default!;
        public string LastName { get; init; } = default!;
        public string PhoneNumber { get; init; } = default!;
        public string Email { get; init; } = default!;
        public string InstagramHandler { get; init; } = default!;
    }
}
