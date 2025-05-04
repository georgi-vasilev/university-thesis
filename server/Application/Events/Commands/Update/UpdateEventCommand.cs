namespace Application.Events.Commands.Update
{
    using ErrorOr;
    using MediatR;

    public record UpdateEventCommand(
        Guid EventId,
        string Name,
        string Description,
        DateOnly Date,
        DateTime StartTime,
        DateTime EndTime,
        Guid VenueId) : IRequest<ErrorOr<UpdateCommandOutputModel>>;
}
