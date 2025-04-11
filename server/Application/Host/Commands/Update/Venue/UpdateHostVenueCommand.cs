namespace Application.Host.Commands.Update.Venue
{
    using ErrorOr;
    using MediatR;

    public record UpdateHostVenueCommand : IRequest<ErrorOr<UpdateHostVenueOutputModel>>
    {
        public Guid Id { get; set; }
        public Guid VenueId { get; set; }
    }
}