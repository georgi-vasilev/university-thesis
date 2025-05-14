namespace Application.Host.Commands.Update.Email
{
    using ErrorOr;
    using MediatR;

    public record UpdateEmailCommand(string Email) : IRequest<ErrorOr<UpdateEmailOutputModel>>;
}
