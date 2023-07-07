

using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using BusinessCard.API;
using BusinessCard.API.Application.Behaviors;
using BusinessCard.API.Extensions;
using BusinessCard.API.Grpc;
using BusinessCard.Domain.Seedwork;
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
    })
    .Services
    .AddCustomDbContext(builder.Configuration)
    .AddCustomSwagger();

builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
    builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
}));
//
// builder.Services.AddAuthentication().AddCertificate(opt =>
// {
//     opt.AllowedCertificateTypes = CertificateTypes.SelfSigned;
//     opt.RevocationMode = X509RevocationMode.NoCheck; // Self-Signed Certs (Development)
//     opt.Events = new CertificateAuthenticationEvents()
//     {
//         OnCertificateValidated = ctx =>
//         {
//             // Write additional Validation  
//             ctx.Success();
//             return Task.CompletedTask;
//         }
//     };
// });


//register mediatr and pipelines
builder.Services.AddMediatR(c => c.RegisterServicesFromAssemblies(typeof(Program).Assembly));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehaviour<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));

//claims middlewares
builder.Services.AddScoped(typeof(ICurrentUser), typeof(CurrentUser));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.WebHost
    .ConfigureKestrel(options =>
    {
        options.Listen(IPAddress.Any, builder.Configuration.GetValue("GRPC_PORT", 5001),
            listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http1;
            });
        options.Listen(IPAddress.Any, builder.Configuration.GetValue("PORT", 80),
            listenOptions =>
                listenOptions.Protocols = HttpProtocols.Http1);


        //options.ListenLocalhost(5001,o => o.Protocols = HttpProtocols.Http2);

        // var config = (IConfiguration)options.ApplicationServices.GetService(typeof(IConfiguration));
        // var cert = new X509Certificate2(config["Certificate:File"],
        //     config["Certificate:Password"]);
        //
        // options.ConfigureHttpsDefaults(h =>
        // {
        //
        //     h.ClientCertificateMode = ClientCertificateMode.AllowCertificate;
        //     h.CheckCertificateRevocation = false;
        //     h.ServerCertificate = cert;
        // });
    });
    

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/V1/swagger.json", "Catalog WebAPI"); });

}

app.MapGrpcService<ClientsService>();
app.MapGrpcService<GreeterService>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
