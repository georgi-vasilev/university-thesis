namespace Application.Host.Commands.Update.InstagramHandler
{
    using Domain.Host.Error;
    using Domain.Host.Repository;
    using ErrorOr;
    using MediatR;

    public class UpdateInstagramHandlerCommandHandler : IRequestHandler<UpdateInstagramHandlerCommand, ErrorOr<UpdateInstagramHandlerOutputModel>>
    {
        private readonly IHostDomainRepository _repository;

        public UpdateInstagramHandlerCommandHandler(IHostDomainRepository repository)
            => _repository = repository;

        public async Task<ErrorOr<UpdateInstagramHandlerOutputModel>> Handle(
            UpdateInstagramHandlerCommand request,
            CancellationToken cancellationToken)
        {
            var host = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (host is null)
            {
                return HostErrors.HostNotFoundError;
            }

            var result = host.UpdateInstagramHandler(request.InstagramHandler);
            if (result.IsError)
            {
                return result.FirstError;
            }

            await _repository.UpdateAsync(host, cancellationToken);

            return new UpdateInstagramHandlerOutputModel(host.Id, host.ContactInfo.InstagramHandler);
        }
    }
}
