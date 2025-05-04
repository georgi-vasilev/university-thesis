namespace Application.Events.Queries.GetEventsByHost
{
    using ErrorOr;
    using GetEvents;
    using MediatR;

    public record GetEventsByHostQuery : IRequest<ErrorOr<List<GetEventsOutputModel>>>
    {
        public Guid Id { get; init; }
    }
}
