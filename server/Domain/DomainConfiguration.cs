namespace Domain
{
    using Common;
    using Domain.Host.Builder;
    using Domain.Order.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using System.Reflection;

    public static class DomainConfiguration
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
            => services
                .AddDomainBuilders()
                .AddDomainServices()
                .AddInitialData()
                .AddScoped<IHostBuilder, HostBuilder>()
                .AddScoped<ITicketBuilder, TicketBuilder>()
                .AddScoped<IContactInfoBuilder, ContactInfoBuilder>();

        private static IServiceCollection AddDomainBuilders(this IServiceCollection services)
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

        private static IServiceCollection AddInitialData(this IServiceCollection services)
         => services
             .Scan(scan => scan
             .FromAssemblies(Assembly.GetExecutingAssembly())
                 .AddClasses(classes => classes
                     .AssignableTo(typeof(IInitialData)), publicOnly: false)
                 .AsImplementedInterfaces()
                 .WithTransientLifetime());
    }
}
