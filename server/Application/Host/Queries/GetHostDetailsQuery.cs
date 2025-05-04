namespace Application.Host.Queries
{
    using ErrorOr;
    using MediatR;

    public record GetHostDetailsQuery : IRequest<ErrorOr<GetHostDetailsOutputModel>>
    {
        public Guid Id { get; init; }
    }
}
