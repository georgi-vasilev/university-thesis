namespace Web
{
    using Application.Events.Commands.Cancel;
    using Application.Services.Contracts.User;
    using Filters;
    using FluentValidation;
    using FluentValidation.AspNetCore;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Services;

    public static class WebConfiguration
    {
        public static IServiceCollection AddWebComponents(this IServiceCollection services)
        {
            services.AddScoped<ICurrentUser, CurrentUserService>();
            services
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters()
                .AddEndpointsApiExplorer()
                .AddValidatorsFromAssemblyContaining<CancelEventCommand>()
                .AddControllers(options =>
                {
                    options.Filters.Add<ErrorOrProblemDetailsFilter>();
                })
                .AddNewtonsoftJson();

            services.Configure<ApiBehaviorOptions>(opts =>
                opts.SuppressModelStateInvalidFilter = true);

            return services;
        }
    }
}

