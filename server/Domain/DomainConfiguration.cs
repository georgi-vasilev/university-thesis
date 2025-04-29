namespace Domain
{
    using Common;
    using Domain.Host.Builder;
    using Domain.Order.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using System.Reflection;


    // This configuration is subject to change over time. 
    // Adding this at the moment as something initial so that I don't forget to do this kind of
    // Abstraction in the future for rest of the layers.
    public static class DomainConfiguration
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
            => services
                .AddBuilders()
                .AddDomainServices()
                .AddScoped<IHostBuilder, HostBuilder>()
                .AddScoped<ITicketBuilder, TicketBuilder>()
                .AddScoped<IContactInfoBuilder, ContactInfoBuilder>();

        private static IServiceCollection AddBuilders(this IServiceCollection services)
            => services
                .Scan(scan => scan
                    .FromAssemblies(Assembly.GetExecutingAssembly())
                    .AddClasses(classes => classes.AssignableTo(typeof(IBuilder<>)), publicOnly: false)
                    .AsMatchingInterface()
                    .WithTransientLifetime());

        private static IServiceCollection AddDomainServices(this IServiceCollection services)
            => services
                .Scan(scan => scan
                    .FromAssemblies(Assembly.GetExecutingAssembly())
                    .AddClasses(classes => classes.AssignableTo(typeof(IDomainService)), publicOnly: false)
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());
    }
}
