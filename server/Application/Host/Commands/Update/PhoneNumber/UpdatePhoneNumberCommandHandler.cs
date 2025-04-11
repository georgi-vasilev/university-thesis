namespace Application.Host.Commands.Update.PhoneNumber
{
    using Domain.Host.Error;
    using Domain.Host.Repository;
    using ErrorOr;
    using MediatR;

    public class UpdatePhoneNumberCommandHandler : IRequestHandler<UpdatePhoneNumberCommand, ErrorOr<UpdatePhoneNumberOutputModel>>
    {
        private readonly IHostDomainRepository _repository;

        public UpdatePhoneNumberCommandHandler(IHostDomainRepository repository)
            => _repository = repository;

        public async Task<ErrorOr<UpdatePhoneNumberOutputModel>> Handle(
            UpdatePhoneNumberCommand request,
            CancellationToken cancellationToken)
        {
            var host = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (host is null)
            {
                return HostErrors.HostNotFoundError;
            }

            var result = host.UpdatePhoneNumber(request.PhoneNumber);
            if (result.IsError)
            {
                return result.FirstError;
            }

            await _repository.UpdateAsync(host, cancellationToken);

            return new UpdatePhoneNumberOutputModel(host.Id, host.ContactInfo.PhoneNumber);
        }
    }
}