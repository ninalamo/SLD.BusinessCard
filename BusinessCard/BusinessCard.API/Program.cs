

using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Autofac.Extensions.DependencyInjection;
using BusinessCard.API;
using BusinessCard.API.Application.Behaviors;
using BusinessCard.API.Application.Commands;
using BusinessCard.API.Application.Commands.UpsertClient;
using BusinessCard.API.Extensions;
using BusinessCard.API.Grpc;
using BusinessCard.API.Interceptors;
using BusinessCard.API.Logging;
using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using BusinessCard.Domain.Seedwork;
using BusinessCard.Infrastructure.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder
    .Services
    .AddGrpc(o =>
    {
        o.EnableDetailedErrors = true;
        o.Interceptors.Add<ServerInterceptor>();
    })
    .Services
    .AddCustomDbContext(builder.Configuration)
    .AddCustomSwagger();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.AddSingleton<ServerInterceptor>();



builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
    builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
}));

builder.Services.AddScoped(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));

//register mediatr and pipelines
builder.Services.AddMediatR(c => c.RegisterServicesFromAssemblies(typeof(Program).Assembly));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehaviour<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));

// Register command and query handlers
builder.Services.AddScoped(typeof(IClientsRepository), typeof(ClientsRepository));
builder.Services.AddScoped<IRequestHandler<AddClientCommand, CommandResult>, AddClientCommandHandler>();
    
// Register notification handlers
// builder.Services.AddScoped(typeof(INotificationHandler<>), typeof(ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler));

// Register validators
builder.Services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(AddClientCommandValidator)));





//claims middlewares
builder.Services.AddScoped(typeof(ICurrentUser), typeof(CurrentUser));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
   

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/V1/swagger.json", "Catalog WebAPI"); });

}

app.MapGrpcService<GreeterService>();
app.MapGrpcService<ClientsService>();

app.MapGet("/", () => "");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
