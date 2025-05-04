namespace Infrastructure
{
    using Application.Common.Contracts;
    using Application.Services.Contracts.Payment;
    using Domain.Event.Repository;
    using Domain.Host.Repository;
    using Domain.Order.Repository;
    using Domain.Venue.Repository;
    using Infrastructure.Persistence;
    using Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Repositories;
    using Repositories.Event;

    //TODO: check how to configure auth
    public static class InfrastructureConfiguration
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
            => services
            .AddDatabase(configuration)
            .AddRepositories()
            .AddServices();

        private static IServiceCollection AddDatabase(
            this IServiceCollection services,
            IConfiguration configuration)
            => services
            .AddDbContext<ApplicationDbContext>(opts =>
                opts.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    sql => sql.MigrationsAssembly(
                        typeof(ApplicationDbContext).Assembly.FullName)))
            .AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>())
            .AddTransient<IInitializer, DatabaseInitializer>();


        private static IServiceCollection AddRepositories(this IServiceCollection services)
            => services
                .AddScoped<IEventDomainRepository, EventRepository>()
                .AddScoped<IHostDomainRepository, HostRepository>()
                .AddScoped<IOrderDomainRepository, OrderRepository>()
                .AddScoped<IVenueDomainRepository, VenueRepository>()
                .AddScoped<IEventQueryRepository, EventQueryRepository>();

        //private static IServiceCollection AddAuth(
        //    this IServiceCollection services,
        //    IConfiguration config)
        //    => services.AddAuthenticationWithAzureAd(config);

        private static IServiceCollection AddServices(this IServiceCollection services)
            => services
                .AddSingleton<IPaymentService, StripePaymentService>();

    }
}