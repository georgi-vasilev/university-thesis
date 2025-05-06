namespace Application.Host.Queries
{
    using Common.Contracts;
    using ErrorOr;
    using MediatR;

    public class GetHostDetailsQueryHandler : IRequestHandler<GetHostDetailsQuery, ErrorOr<GetHostDetailsOutputModel>>
    {
        private IHostQueryRepository _repository;

        public GetHostDetailsQueryHandler(IHostQueryRepository repository) 
            => _repository = repository;

        public async Task<ErrorOr<GetHostDetailsOutputModel>> Handle(
            GetHostDetailsQuery request,
            CancellationToken cancellationToken) 
            => await _repository.GetDetailsAsync(
                request.Id,
                cancellationToken);
    }
}
