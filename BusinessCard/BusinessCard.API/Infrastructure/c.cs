using BusinessCard.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BusinessCard.API.Infrastructure;


public class LokiContextFactory : IDesignTimeDbContextFactory<LokiContext>
{
    public LokiContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        var connectionString = config.GetConnectionString("DefaultConnection");

        var optionsBuilder = new DbContextOptionsBuilder<LokiContext>();
        optionsBuilder.UseSqlServer(connectionString, x => x.MigrationsAssembly("Taaldc.Catalog.Infrastructure"));

        return new LokiContext(optionsBuilder.Options, new NoMediator(), null);
    }
}


public class NoMediator : IMediator
{
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = new CancellationToken()) where TRequest : IRequest
    {
        throw new NotImplementedException();
    }

    public Task<object?> Send(object request, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request,
        CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<object?> CreateStream(object request, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task Publish(object notification, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = new CancellationToken()) where TNotification : INotification
    {
        throw new NotImplementedException();
    }
}