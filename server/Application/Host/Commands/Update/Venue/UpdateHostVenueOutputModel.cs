namespace Application.Host.Commands.Update.Venue
{
    public record UpdateHostVenueOutputModel
    {
        public UpdateHostVenueOutputModel(
            Guid id,
            Guid venueId)
        {
            Id = id;
            VenueId = venueId;
        }

        public Guid Id { get; init; }
        public Guid VenueId { get; init; }
    }
}
