namespace Application.Events.Commands.Create
{
    using ErrorOr;
    using MediatR;

    public record CreateEventCommand(
        string Name,
        string Description,
        DateOnly Date,
        DateTime StartTime,
        DateTime EndTime,
        Guid VenueId,
        int Capacity) : IRequest<ErrorOr<CreateEventOutputModel>>;
}
