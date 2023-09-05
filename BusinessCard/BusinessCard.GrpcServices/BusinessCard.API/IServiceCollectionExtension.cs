using System.Reflection;
using BusinessCard.API.Application.Behaviors;
using BusinessCard.API.Application.Commands;
using BusinessCard.API.Application.Commands.AddClient;
using BusinessCard.API.Application.Commands.UpsertClient;
using BusinessCard.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace BusinessCard.API;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services
            .AddDbContext<LokiContext>(options =>
                {
                    options.UseSqlServer(connectionString,
                        sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(typeof(LokiContext).Assembly.FullName);
                            sqlOptions.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
                        });
                } //Showing explicitly that the DbContext is shared across the HTTP request scope (graph of objects started in the HTTP request)
            );
        return services;
    }
    
    public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("V1", new OpenApiInfo
            {
                Version = "V1",
                Title = "WebAPI",
                Description = "Business Card Grpc Web API"
            });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Description = "Bearer Authentication with JWT Token",
                Type = SecuritySchemeType.Http
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new List<string>()
                }
            });
        });

        return services;
    }

    public static IServiceCollection AddMediatorBundles(this IServiceCollection services)
    {

        //register mediatr and pipelines
        services.AddMediatR(c => c.RegisterServicesFromAssemblies(typeof(AddClientCommand).Assembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));

        // Register command and query handlers
        services.AddScoped<IRequestHandler<AddClientCommand, CommandResult>, AddClientCommandHandler>();

        // Register validators
        services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(AddClientCommandValidator)));
        return services;
    }
}