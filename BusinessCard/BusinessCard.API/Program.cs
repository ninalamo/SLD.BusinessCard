using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Autofac.Extensions.DependencyInjection;
using BusinessCard.API;
using BusinessCard.API.Application.Behaviors;
using BusinessCard.API.Application.Commands;
using BusinessCard.API.Application.Commands.UpsertClient;
using BusinessCard.API.Application.Queries;
using BusinessCard.API.Application.Queries.GetClients;
using BusinessCard.API.Extensions;
using BusinessCard.API.Grpc;
using BusinessCard.API.Interceptors;
using BusinessCard.API.Logging;
using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using BusinessCard.Domain.Seedwork;
using BusinessCard.Infrastructure;
using BusinessCard.Infrastructure.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//Add Serilog
builder.Host.UseSerilog((ctx, lc) => lc
    .Enrich.FromLogContext()
    .MinimumLevel.Verbose()
    .WriteTo.Console()
    .WriteTo.Debug()
    .WriteTo.Seq("http://localhost:5341"));

// Add services to the container.
builder
    .Services
    .AddSingleton<ServerInterceptor>()
    .AddGrpc(o =>
    {
        o.EnableDetailedErrors = true;
        o.Interceptors.Add<ServerInterceptor>();
    });

builder.Services
    .AddCustomDbContext(builder.Configuration)
    .AddCustomSwagger()
    .AddCors(o => o.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
    }))
    .AddMediatorBundles();

//claims middlewares
builder.Services.AddScoped(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
builder.Services.AddScoped(typeof(ICurrentUser), typeof(CurrentUser));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//register queries
builder.Services.AddScoped(typeof(IClientsRepository), typeof(ClientsRepository));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddScoped<IClientQueries>(i => new ClientQueries(connectionString));

// reverse proxy headers forwarding config
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor |
        Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto;
});

// enable Systemd integration
builder.Host.UseSystemd();

var app = builder.Build();

// enable proxy headers forwarding
app.UseForwardedHeaders();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/V1/swagger.json", "Catalog WebAPI"); });
}

using (var scope = app.Services.CreateAsyncScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<LokiContext>();
    await ctx.Database.MigrateAsync();
}

app.MapGrpcService<GreeterService>();
app.MapGrpcService<ClientsService>();

app.MapGet("/", () => "");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
