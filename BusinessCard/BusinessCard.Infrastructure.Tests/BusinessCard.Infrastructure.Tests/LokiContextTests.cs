using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using BusinessCard.Domain.Seedwork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using Shouldly;

namespace BusinessCard.Infrastructure.Tests;

public class LokiContextTests : IDisposable
{
    private readonly LokiContext _dbContext;
    private readonly IDbContextTransaction _transaction;

    public LokiContextTests()
    {
        // Set up the DbContext with an in-memory database
        var options = new DbContextOptionsBuilder<LokiContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var mockMediator = new Mock<IMediator>();
        var mockCurrentUser = new Mock<ICurrentUser>();

        _dbContext = new LokiContext(options, mediator: mockMediator.Object, currentUser: mockCurrentUser.Object);
        _dbContext.Database.EnsureCreated();

        _transaction = _dbContext.Database.BeginTransaction();
    }

    [Fact]
    public async Task SaveEntitiesAsync_ShouldSaveChanges()
    {
        // Arrange
        var client = new Client("Test Client", true, "Test Industry");
        _dbContext.Clients.Add(client);

        // Act
        var savedEntities = await _dbContext.SaveEntitiesAsync();

        // Assert
        savedEntities.ShouldBeGreaterThan(0); // Ensure that entities were saved
    }

    [Fact]
    public async Task BeginTransactionAsync_ShouldStartTransaction()
    {
        // Act
        var transaction = await _dbContext.BeginTransactionAsync();

        // Assert
        transaction.ShouldNotBeNull(); // Ensure that a transaction was started
    }

    [Fact]
    public void RollbackTransaction_ShouldRollback()
    {
        // Arrange
        var transaction = _dbContext.Database.BeginTransaction();

        // Act
        _dbContext.RollbackTransaction();

        // Assert
        transaction.GetDbTransaction().ShouldBeNull(); // Ensure that the transaction was rolled back
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _dbContext?.Dispose();
    }
}