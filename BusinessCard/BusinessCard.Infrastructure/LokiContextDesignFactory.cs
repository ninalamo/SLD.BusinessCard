using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BusinessCard.Infrastructure;

public class LokiContextDesignFactory : IDesignTimeDbContextFactory<LokiContext>
{
    public LokiContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<LokiContext>()
            .UseSqlServer("Server=localhost;Database=kardb;User Id=sa;Password=someThingComplicated1234;",
                x => x.MigrationsAssembly(Assembly.GetAssembly(typeof(LokiContext))?.GetName().Name));

        return new LokiContext(optionsBuilder.Options, new NoMediator(), null);
    }
        
        
    private class NoMediator : IMediator
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
}