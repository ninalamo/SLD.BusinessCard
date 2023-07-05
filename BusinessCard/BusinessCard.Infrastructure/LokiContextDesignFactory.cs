﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BusinessCard.Infrastructure;

public class LokiContextDesignFactory : IDesignTimeDbContextFactory<LokiContext>
{
    public LokiContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<LokiContext>()
            .UseSqlServer("", x => x.MigrationsAssembly(nameof(BusinessCard.Infrastructure)));

        return new LokiContext(optionsBuilder.Options, new NoMediator(), null);
    }
    
    private class NoMediator : IMediator
    {
        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = new CancellationToken())
        {
            return default;
        }

        public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = new CancellationToken()) where TRequest : IRequest
        {
            return default;
        }

        public Task<object?> Send(object request, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.FromResult(default(object));
        }

        public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return default;
        }

        public IAsyncEnumerable<object?> CreateStream(object request, CancellationToken cancellationToken = new CancellationToken())
        {
            return default;
        }

        public Task Publish(object notification, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = new CancellationToken()) where TNotification : INotification
        {
            return Task.CompletedTask;
        }
    }
}


