namespace Application.Events.Queries.GetEventDetails
{
    using Application.Common.Contracts;
    using ErrorOr;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetEventDetailsQueryHandler : IRequestHandler<GetEventDetailsQuery, ErrorOr<GetEventDetailsOutputModel>>
    {
        private readonly IEventQueryRepository _repository;

        public GetEventDetailsQueryHandler(IEventQueryRepository repository) 
            => _repository = repository;

        public async Task<ErrorOr<GetEventDetailsOutputModel>> Handle(GetEventDetailsQuery request, CancellationToken cancellationToken) 
            => await _repository.GetDetailsAsync(request.Id, cancellationToken);
    }
}
