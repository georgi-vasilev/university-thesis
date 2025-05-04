namespace Application.Events.Queries.GetEventDetails
{
    using ErrorOr;
    using MediatR;

    public record GetEventDetailsQuery : IRequest<ErrorOr<GetEventDetailsOutputModel>>
    {
        public Guid Id { get; init; }
    }
}
