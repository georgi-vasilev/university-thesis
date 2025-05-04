namespace Application.Common.Contracts
{
    using ErrorOr;
    using Host.Queries;

    public interface IHostQueryRepository
    {
        Task<ErrorOr<GetHostDetailsOutputModel>> GetDetailsAsync(
            Guid hostId,
            CancellationToken cancellationToken);
    }

}
