namespace Application.Events.Queries.GetEventDetails
{
    using ErrorOr;
    using MediatR;

    public record GetEventDetailsQuery : IRequest<ErrorOr<GetEventDetailsOutputModel>>
    {
        public GetEventDetailsQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; init; }
    }
}
