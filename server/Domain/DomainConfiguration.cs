namespace Domain
{
    using Common;
    using Event.Service;
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
                .AddDomainServices();

        private static IServiceCollection AddBuilders(this IServiceCollection services)
            => services
                .Scan(scan => scan
                    .FromAssemblies(Assembly.GetExecutingAssembly())
                    .AddClasses(classes => classes.AssignableTo(typeof(IBuilder<>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());

        private static IServiceCollection AddDomainServices(this IServiceCollection services)
            => services
                .Scan(scan => scan
                    .FromAssemblies(Assembly.GetExecutingAssembly())
                    .AddClasses(classes => classes.AssignableTo(typeof(IEventHostService)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());
    }
}
