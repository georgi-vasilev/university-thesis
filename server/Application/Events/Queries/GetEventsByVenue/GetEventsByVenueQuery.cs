namespace Application.Events.Queries.GetEventsByVenue
{
    using ErrorOr;
    using GetEvents;
    using MediatR;

    public record GetEventsByVenueQuery : IRequest<ErrorOr<List<GetEventsOutputModel>>>
    {
        public Guid Id { get; init; }
    }
}
