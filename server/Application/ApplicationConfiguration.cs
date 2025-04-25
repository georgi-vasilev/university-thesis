namespace Application
{
    using Common.Behaviours;
    using FluentValidation;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System.Reflection;

    // Again this configuration is subject to change
    // And mostly likely is in complete at the moment.
    public static class ApplicationConfiguration
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services,
            IConfiguration configuration)
            => services
                .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

    }
}
