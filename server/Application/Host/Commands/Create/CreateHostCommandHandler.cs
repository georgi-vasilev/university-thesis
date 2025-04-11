namespace Application.Host.Commands.Create
{
    using Domain.Host.Builder;
    using Domain.Host.Repository;
    using ErrorOr;
    using MediatR;

    public class CreateHostCommandHandler : IRequestHandler<CreateHostCommand, ErrorOr<CreateHostOutputModel>>
    {
        private readonly IHostBuilder _hostBuilder;
        private readonly IHostDomainRepository _repository;
        private readonly IContactInfoBuilder _contactInfoBuilder;

        public CreateHostCommandHandler(
            IHostBuilder hostBuilder,
            IHostDomainRepository repository,
            IContactInfoBuilder contactInfoBuilder)
        {
            _hostBuilder = hostBuilder;
            _repository = repository;
            _contactInfoBuilder = contactInfoBuilder;
        }

        public async Task<ErrorOr<CreateHostOutputModel>> Handle(
            CreateHostCommand request,
            CancellationToken cancellationToken)
        {
            var contactInfoBuildResult = _contactInfoBuilder
                .WithFirstName(request.FirstName)
                .WithLastName(request.LastName)
                .WithPhoneNumber(request.PhoneNumber)
                .WithEmail(request.Email)
                .WithInstagramHandler(request.InstagramHandler)
                .Build();

            if (contactInfoBuildResult.IsError)
            {
                return contactInfoBuildResult.FirstError;
            }

            var contactInfo = contactInfoBuildResult.Value;

            var hostBuildResult = _hostBuilder
                .WithContactInfo(contactInfo)
                .Build();

            if (hostBuildResult.IsError)
            {
                return hostBuildResult.FirstError;
            }

            var host = hostBuildResult.Value;
            await _repository.AddAsync(host, cancellationToken);

            return new CreateHostOutputModel(
                host.ContactInfo.FullName,
                host.ContactInfo.PhoneNumber,
                host.ContactInfo.Email,
                host.ContactInfo.InstagramHandler);
        }
    }
}
