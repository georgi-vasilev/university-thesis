namespace Domain.Host.Builder
{
    using ErrorOr;

    public interface IHostBuilder
    {
        IHostBuilder WithContactInfo(ContactInfo contactInfo);
        IHostBuilder WithVenueId(Guid venueId);
        IHostBuilder WithId(Guid id);
        ErrorOr<Host> Build();
    }
}
