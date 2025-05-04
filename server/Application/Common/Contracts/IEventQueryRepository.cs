namespace Application.Common.Contracts
{
    using Application.Events.Queries.GetEventDetails;
    using ErrorOr;

    public interface IEventQueryRepository
    {
        Task<ErrorOr<GetEventDetailsOutputModel>> GetDetailsAsync(Guid eventId, CancellationToken cancelletionToken);
    }
}
