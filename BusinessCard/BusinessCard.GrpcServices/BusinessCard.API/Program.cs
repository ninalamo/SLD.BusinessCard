using BusinessCard.API.Application.Common.Interfaces;
using BusinessCard.API.Extensions;
using BusinessCard.Application.Application.Common.Interfaces;
using BusinessCard.Application.Application.Queries;
using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using BusinessCard.GrpcServices.Services;
using BusinessCard.Infrastructure.Repositories;
using Microsoft.AspNetCore.Server.Kestrel.Core;
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

if (System.OperatingSystem.IsMacOS())
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        // Setup a HTTP/2 endpoint without TLS.
        //5050 -- see launchsettings.json for http URL
        options.ListenLocalhost(5050, o => o.Protocols = HttpProtocols.Http2);
    });
}

builder.Services
    .AddCustomDbContext(builder.Configuration)
    .AddCustomSwagger()
    .AddMediatorBundles();

//claims middlewares
builder.Services.AddScoped(typeof(ICurrentUser), typeof(CurrentUser));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//register queries and repositories
builder.Services.AddScoped<IClientQueries,ClientQueries>();
builder.Services.AddScoped(typeof(IClientsRepository), typeof(ClientsRepository));


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
    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/V1/swagger.json", "BusinessCard Grpc Web API"); });
}

await using var scope = app.Services.CreateAsyncScope();
{
    var ctx = scope.ServiceProvider.GetRequiredService<LokiContext>();
    await ctx.Database.MigrateAsync();
}

app.MapGrpcService<ClientsService>();
app.MapGrpcService<KardsService>();

app.UseCors(b=> b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.MapGet("/", () => "");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
