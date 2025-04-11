namespace Application.Host.Commands.Update.PhoneNumber
{
    using ErrorOr;
    using MediatR;

    public record UpdatePhoneNumberCommand : IRequest<ErrorOr<UpdatePhoneNumberOutputModel>>
    {
        public Guid Id { get; set; }
        public string PhoneNumber { get; set; } = default!;
    }
}