namespace Infrastructure
{
    using Application.Common;
    using Application.Common.Contracts;
    using Application.Services.Contracts.Payment;
    using Authentication;
    using Domain.Buyer.Repository;
    using Domain.Event.Repository;
    using Domain.Host.Repository;
    using Domain.Order.Repository;
    using Domain.Venue.Repository;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using Persistence;
    using Repositories;
    using Repositories.Event;
    using Repositories.Host;
    using Services;
    using System.Text;

    public static class InfrastructureConfiguration
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
            => services
            .AddDatabase(configuration)
            .AddRepositories()
            .AddAuth(configuration)
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
                .AddScoped<IBuyerDomainRepository, BuyerRepository>()
                .AddScoped<IEventQueryRepository, EventQueryRepository>()
                .AddScoped<IHostQueryRepository, HostQueryRepository>();

        private static IServiceCollection AddAuth(
            this IServiceCollection services,
            IConfiguration config)
        {
            services
                .Configure<ApplicationSettings>(config.GetSection(nameof(ApplicationSettings)));

            var settings = config
                .GetSection(nameof(ApplicationSettings))
                .Get<ApplicationSettings>();

            if (settings is null || string.IsNullOrWhiteSpace(settings.Secret))
            {
                throw new InvalidOperationException("ApplicationSettings section is missing or incomplete in configuration.");
            }

            var key = Encoding.UTF8.GetBytes(settings.Secret);

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = settings.Issuer,
                        ValidateAudience = true,
                        ValidAudience = settings.Audience,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });

            services
                .AddAuthorizationBuilder()
                .AddPolicy("HostOnly", policy => policy.RequireRole("Host"))
                .AddPolicy("BuyerOnly", policy => policy.RequireRole("Buyer"));

            services
                .AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    // this is for development purposes :)
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 1;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredUniqueChars = 0;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
            => services
                .AddSingleton<IPaymentService, StripePaymentService>()
                .AddScoped<IAuthenticationService, AuthenticationService>();

    }
}