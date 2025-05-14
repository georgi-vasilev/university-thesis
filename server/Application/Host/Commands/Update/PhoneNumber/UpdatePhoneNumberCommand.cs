namespace Application.Host.Commands.Update.PhoneNumber
{
    using ErrorOr;
    using MediatR;

    public record UpdatePhoneNumberCommand(string PhoneNumber) : IRequest<ErrorOr<UpdatePhoneNumberOutputModel>>;
}