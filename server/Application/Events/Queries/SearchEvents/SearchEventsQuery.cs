namespace Application.Events.Queries.SearchEvents
{
    using ErrorOr;
    using GetEvents;
    using MediatR;

    public record SearchEventsQuery : IRequest<ErrorOr<List<GetEventsOutputModel>>>
    {
        public string SearchTerm { get; init; } = default!;
    }
}
