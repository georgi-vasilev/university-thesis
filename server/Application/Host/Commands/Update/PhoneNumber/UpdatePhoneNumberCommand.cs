namespace Application.Host.Commands.Update.PhoneNumber
{
    using ErrorOr;
    using MediatR;

    public record UpdatePhoneNumberCommand : IRequest<ErrorOr<UpdatePhoneNumberOutputModel>>
    {
        public Guid Id { get; init; }
        public string PhoneNumber { get; init; } = default!;
    }
}