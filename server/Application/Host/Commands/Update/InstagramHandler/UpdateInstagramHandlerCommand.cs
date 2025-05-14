namespace Application.Host.Commands.Update.InstagramHandler
{
    using ErrorOr;
    using MediatR;

    public record UpdateInstagramHandlerCommand(string InstagramHandler) : IRequest<ErrorOr<UpdateInstagramHandlerOutputModel>>;
}