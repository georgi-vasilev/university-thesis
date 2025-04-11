namespace Application.Host.Commands.Update.InstagramHandler
{
    using ErrorOr;
    using MediatR;

    public record UpdateInstagramHandlerCommand : IRequest<ErrorOr<UpdateInstagramHandlerOutputModel>>
    {
        public Guid Id { get; set; }
        public string InstagramHandler { get; init; } = default!;
    }
}