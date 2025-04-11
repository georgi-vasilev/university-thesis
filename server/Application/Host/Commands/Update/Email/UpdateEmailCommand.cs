namespace Application.Host.Commands.Update.Email
{
    using ErrorOr;
    using MediatR;

    public record UpdateEmailCommand : IRequest<ErrorOr<UpdateEmailOutputModel>>
    {
        public Guid Id { get; init; }
        public string Email { get; init; } = default!;
    }
}
