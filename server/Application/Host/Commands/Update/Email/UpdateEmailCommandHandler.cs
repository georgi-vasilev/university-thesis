namespace Application.Host.Commands.Update.Email
{
    using Domain.Host.Error;
    using Domain.Host.Repository;
    using ErrorOr;
    using MediatR;

    public class UpdateEmailCommandHandler : IRequestHandler<UpdateEmailCommand, ErrorOr<UpdateEmailOutputModel>>
    {
        private readonly IHostDomainRepository _repository;

        public UpdateEmailCommandHandler(IHostDomainRepository repository) 
            => _repository = repository;

        public async Task<ErrorOr<UpdateEmailOutputModel>> Handle(
            UpdateEmailCommand request,
            CancellationToken cancellationToken)
        {
            var host = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (host is null) 
            {
                return HostErrors.HostNotFoundError;
            }

            var result = host.UpdateEmail(request.Email);
            if (result.IsError)
            {
                return result.FirstError;
            }

            await _repository.UpdateAsync(host, cancellationToken);

            return new UpdateEmailOutputModel(host.Id, host.ContactInfo.Email);
        }
    }
}
