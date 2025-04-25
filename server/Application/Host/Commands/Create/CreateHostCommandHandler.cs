namespace Application.Host.Commands.Create
{
    using Domain.Host.Builder;
    using Domain.Host.Error;
    using Domain.Host.Repository;
    using ErrorOr;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel;

    public class CreateHostCommandHandler : IRequestHandler<CreateHostCommand, ErrorOr<CreateHostOutputModel>>
    {
        private readonly IHostBuilder _hostBuilder;
        private readonly IContactInfoBuilder _contactInfoBuilder;
        private readonly IHostDomainRepository _repository;
        private readonly ILogger<CreateHostCommandHandler> _logger;

        public CreateHostCommandHandler(
            IHostBuilder hostBuilder,
            IContactInfoBuilder contactInfoBuilder,
            IHostDomainRepository repository,
            ILogger<CreateHostCommandHandler> logger)
        {
            _hostBuilder = hostBuilder;
            _contactInfoBuilder = contactInfoBuilder;
            _repository = repository;
            _logger = logger;
        }

        public async Task<ErrorOr<CreateHostOutputModel>> Handle(
            CreateHostCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CreateHostCommand: {FirstName} {LastName}", request.FirstName, request.LastName);

            var contactInfoBuildResult = _contactInfoBuilder
                .WithFirstName(request.FirstName)
                .WithLastName(request.LastName)
                .WithPhoneNumber(request.PhoneNumber)
                .WithEmail(request.Email)
                .WithInstagramHandler(request.InstagramHandler)
                .Build();

            if (contactInfoBuildResult.IsError)
            {
                _logger.LogWarning("ContactInfoBuilder failed: {ErrorCode}", contactInfoBuildResult.FirstError.Code);
                return contactInfoBuildResult.FirstError;
            }

            var contactInfo = contactInfoBuildResult.Value;

            var hostBuildResult = _hostBuilder
                .WithContactInfo(contactInfo)
                .Build();

            if (hostBuildResult.IsError)
            {
                _logger.LogWarning("HostBuilder failed: {ErrorCode}", hostBuildResult.FirstError.Code);
                return hostBuildResult.FirstError;
            }

            var host = hostBuildResult.Value;

            try
            {
                await _repository.AddAsync(host, cancellationToken);
                _logger.LogInformation("Host {HostId} created successfully.", host.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Host for {FullName}.", contactInfo.FullName);
                return HostErrors.UnexpectedError;
            }

            return new CreateHostOutputModel(
                host.ContactInfo.FullName,
                host.ContactInfo.PhoneNumber,
                host.ContactInfo.Email,
                host.ContactInfo.InstagramHandler);
        }
    }
}
